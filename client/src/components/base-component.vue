<template>
  <div class="container">
    <div class="m-3 w-100 text-center"><h1 class="fw-bold text-decoration-underline">Publicaciones de Propiedades</h1></div>
    <div class="m-4 w-100 d-flex justify-content-center "><button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#newPublicacion"><i class="fa-solid fa-plus"></i> Nueva Publicacion</button></div>
    <div v-if="errors.length > 0" class="alert alert-danger">
      <ul>
        <li v-for="error in errors" v-bind:key="error"><p class="text-danger">{{ error }}</p></li>
      </ul>
    </div>
    <div v-if="cargando" class=" text-center ">
      <div  class="spinner-border mt-3" role="status">
        <span class="sr-only"></span>
      </div>
    </div>
    <div v-if="!cargando" class="row">
      <div class="col-md-12 " >
        <div class="mt-3 mb-3" v-for="publicacion in publicaciones" :key="publicacion.id">
          <div class="card">
            <div class="card-header justify-content-between d-flex ">
              <h4 class="fw-bold">{{ publicacion.descripcion }}</h4>
              <div >
                <button type="button" class="btn btn-danger me-3"  @click="eliminarPublicacion(publicacion.publicacionId)"><i class="fa-solid fa-trash"></i></button>
                <button type="button" class="btn btn-primary" @click="editarPublicacion(publicacion.publicacionId)" data-bs-toggle="modal" data-bs-target="#newPublicacion"><i class="fa-solid fa-pen"></i></button>
              </div>
            </div>
            <div class="card-body">
              <h5 class="card-title ">{{ tipoPropiedadConfig(publicacion.tipoPropiedad) }} - {{ tipoOperacionConfig(publicacion.tipoOperacion) }}</h5>
              <ul class="list-group list-group-flush">
                <li class="list-group-item">Ambientes: {{ publicacion.ambientes }}</li>
                <li class="list-group-item">M2: {{ publicacion.m2 }}</li>
                <li class="list-group-item">Antiguedad: {{ formatearAntiguedad(publicacion.antiguedad)}}</li>
                <li class="list-group-item">Coordenadas Ubicación: {{ publicacion.coordenadasUbicacion }}</li>
              </ul>
            </div>
            <div class="card-footer">
              <ul class="d-flex gap-2">
                <div  v-for="imagen in publicacion.imagenes" :key="imagen.id">
                  <img :src="'data:image/jpeg;base64,' + imagen.contenido" alt="Imagen" class="img-fluid" width="300px">
                </div>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>


  <!-- Modal -->
<div class="modal fade" id="newPublicacion" tabindex="-1" aria-labelledby="newPublicacionLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h1 class="modal-title fs-5" id="newPublicacionLabel">Nueva Publicación</h1>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" @click="vaciarPropiedades()" ></button>
      </div>
      <div class="modal-body">
        
        <div class="formulario">
          <div class="mb-3">
            <label for="descripcion" class="form-label">Descripción</label>
            <input v-model="descripcion" type="text" class="form-control" id="descripcion" placeholder="Descripción">
          </div>
          <div class="mb-3">
            <label for="TipoOperacion" class="form-label">Tipo Operacion</label>
            <select v-model="tipoOperacion" class="form-select" aria-label="TipoOperacion">
              <option v-for="operacion in tipoOperacionEnum" :key="operacion.value" :value="operacion.value">{{ operacion.name }}</option>
            </select>        
            <label for="TipoPropiedad" class="form-label">Tipo Propiedad</label>
            <select v-model="tipoPropiedad" class="form-select" aria-label="TipoPropiedad">
              <option v-for="propiedad in tipoPropiedadEnum" :key="propiedad.value" :value="propiedad.value">{{ propiedad.name }}</option>
            </select>
          </div>
          <div class="mb-3">
            <label for="ambientes" class="form-label">Ambientes</label>
            <input v-model="ambientes" type="number" class="form-control" id="ambientes" placeholder="Ambientes">
          </div>
          <div class="mb-3">
            <label for="coordenadas" class="form-label">Coordenadas</label>
            <input v-model="coordenadasUbicacion" type="text" class="form-control" id="coordenadas" placeholder="coordenadas">
          </div>
          <div class="mb-3">
            <label for="m2" class="form-label">M2</label>
            <input v-model="m2" type="number" class="form-control" id="m2" placeholder="M2">
          </div>
          <div class="mb-3">
            <label for="antiguedad" class="form-label">antiguedad</label>
            <input v-model="antiguedad" type="date" class="form-control" id="antiguedad" placeholder="antiguedad">
          </div>
          <div class="mb-3 d-flex flex-column">
            <label for="tipoPropiedad" class="form-label">Cargar Imagenes</label>
            <input type="file" @change="onFileChange" accept="image/jpeg" multiple>
          </div>
          <div v-if="isEditar && imagenesPrecargadas != null ">
            <div class="d-flex gap-2">
             <div v-for="imagen in imagenesPrecargadas" :key="imagen.imagenId">
               <img :src="'data:image/jpeg;base64,' + imagen.contenido" alt="Imagen" class="img-fluid" width="100px">
             </div>
            </div>
           </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" @click="vaciarPropiedades()">Cancelar</button>
        <button v-if="!isEditar"  type="button" class="btn btn-primary" @click="agregarPublicacion()" data-bs-dismiss="modal" >Añadir</button>
        <button v-if="isEditar"  type="button" class="btn btn-success" @click="confirmarEditar()" data-bs-dismiss="modal">Confirmar Cambios</button>
      </div>
    </div>
  </div>
</div>

</template>



<script>
// Importa el servicio que realiza llamadas a la API
import apicall from "../services/propiedadesServices.ts"

export default {
  // Datos del componente
  data() {
    return {
      // Bandera para indicar si se está editando una publicación
      isEditar: false,
      // Lista de publicaciones
      publicaciones: [],
      // Lista de imágenes para una nueva publicación
      imagenes: [],
      // Enumeraciones para tipos de operación y propiedad
      tipoOperacionEnum: [],
      tipoPropiedadEnum: [],
      // Identificador de la publicación en edición
      idPublicacion: null,
      // Detalles de la publicación en edición
      descripcion: null,
      tipoOperacion: 0,
      tipoPropiedad: 0,
      ambientes: 0,
      m2: 0,
      antiguedad: null,
      coordenadasUbicacion: null,
      imagenesPrecargadas: [],
      // Lista de errores durante la operación
      errors: [],
      // Bandera para mostrar el spinner de carga
      cargando: false
    }
  },
  // Método llamado al montar el componente
  mounted() {
    // Cargar configuraciones y datos iniciales
    this.cargarDatos();
    this.cargarConfig();
  },
  // Métodos del componente
  methods: {
    // Carga las configuraciones necesarias
    cargarConfig() {
      this.cargando = true;
      // Llamada a la API para obtener configuraciones
      apicall.getConfigs().then((response) => {
        // Almacena las enumeraciones de tipos de operación y propiedad
        this.tipoOperacionEnum = response.tipoOperacionEnum;
        this.tipoPropiedadEnum = response.tipoPropiedadEnum;
        this.cargando = false;
      }).catch((error) => {
        console.log(error);
      });
    },
    // Elimina una publicación por su identificador
    eliminarPublicacion(id) {
      apicall.deletePublicacion(id).then(() => {
        // Recarga la lista de publicaciones después de la eliminación
        this.cargarDatos();
      }).catch((error) => {
        console.log(error);
      });
    },
    // Carga la lista de publicaciones
    cargarDatos() {
      apicall.getPublicaciones().then((response) => {
        // Almacena la lista de publicaciones obtenida de la API
        this.publicaciones = response;
      }).catch((error) => {
        console.log(error);
      });
    },
    // Maneja el cambio de archivos al seleccionar imágenes
    onFileChange(e) {
      // ... (código para procesar archivos y almacenar imágenes)
    },
    // Agrega una nueva publicación
    agregarPublicacion() {
      // Inicializa la lista de errores
      this.errors = [];

      // Validaciones de campos obligatorios y formatos
      if (!this.descripcion || !this.tipoOperacion || !this.tipoPropiedad || !this.ambientes || !this.m2 || !this.antiguedad || !this.coordenadasUbicacion) {
        this.errors.push("Todos los campos marcados con * son obligatorios.");
      }

      if (this.ambientes <= 0 || this.m2 <= 0) {
        this.errors.push("Ambientes y M2 deben ser mayores que cero.");
      }

      if (this.antiguedad == null) {
        this.errors.push("Antiguedad es obligatoria.");
      }

      // Si hay errores, se detiene la operación
      if (this.errors.length > 0) {
        return;
      }

      // Objeto con los datos de la nueva publicación
      const publicacion = {
        descripcion: this.descripcion,
        tipoOperacion: this.tipoOperacion,
        tipoPropiedad: this.tipoPropiedad,
        ambientes: this.ambientes,
        m2: this.m2,
        antiguedad: this.antiguedad,
        coordenadasUbicacion: this.coordenadasUbicacion,
        imagenes: this.imagenes,
      };

      // Llamada a la API para agregar la nueva publicación
      apicall.addPublicacion(publicacion)
        .then(() => {
          // Limpia los campos y recarga la lista de publicaciones
          this.vaciarPropiedades();
          this.cargarDatos();
        })
        .catch((error) => {
          console.log(error);
        });
    },
    // Obtiene los datos de una publicación para editar
    editarPublicacion(id) {
      // Llamada a la API para obtener los detalles de la publicación en edición
      apicall.getPublicacion(id).then(response => {
        // Establece los datos de la publicación en el formulario de edición
        this.isEditar = true;
        this.idPublicacion = response.publicacionId;
        this.descripcion = response.descripcion;
        this.tipoOperacion = response.tipoOperacion;
        this.tipoPropiedad = response.tipoPropiedad;
        this.ambientes = response.ambientes;
        this.m2 = response.m2;
        this.antiguedad = response.antiguedad.toString().split('T')[0];
        this.coordenadasUbicacion = response.coordenadasUbicacion;
        this.imagenesPrecargadas = response.imagenes;
      }).catch((error) => {
        console.log(error);
      });
    },
    // Confirma la edición de una publicación
    confirmarEditar() {
      // Objeto con los datos actualizados de la publicación
      const publicacion = {
        descripcion: this.descripcion,
        tipoOperacion: this.tipoOperacion,
        tipoPropiedad: this.tipoPropiedad,
        ambientes: this.ambientes,
        m2: this.m2,
        antiguedad: this.antiguedad,
        coordenadasUbicacion: this.coordenadasUbicacion,
        imagenes: this.imagenes
      };

      // Llamada a la API para actualizar la publicación
      apicall.updatePublicacion(this.idPublicacion, publicacion).then(() => {
        // Limpia los campos y recarga la lista de publicaciones
        this.vaciarPropiedades();
        this.cargarDatos();
      }).catch((error) => {
        console.log(error);
      });
    },
    // Limpia los campos del formulario y reinicia banderas
    vaciarPropiedades() {
      this.isEditar = false;
      this.descripcion = null;
      this.tipoOperacion = null;
      this.tipoPropiedad = null;
      this.ambientes = null;
      this.m2 = null;
      this.antiguedad = null;
      this.coordenadasUbicacion = null;
      this.imagenes = [];
      this.imagenesPrecargadas = [];
      this.idPublicacion = null;
      this.errors = [];
    },
    // Obtiene la descripción del tipo de operación por su clave
    tipoOperacionConfig(key) {
      return this.tipoOperacionEnum.find(x => x.value === key).name;
    },
    // Obtiene la descripción del tipo de propiedad por su clave
    tipoPropiedadConfig(key) {
      return this.tipoPropiedadEnum.find(x => x.value === key).name;
    },
    // Formatea la antigüedad en años a partir de la fecha de antigüedad
    formatearAntiguedad(fecha) {
      if (fecha) {
        const fechaObj = new Date(fecha);
        const añoActual = new Date().getFullYear();
        const añoAntiguedad = fechaObj.getFullYear();
        const antiguedad = añoActual - añoAntiguedad;
        return `${antiguedad} años`;
      }
      return 'N/A';
    }
  }
}
</script>


<style scoped>
</style>
