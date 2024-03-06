export default {
    // Obtener todas las publicaciones
    getPublicaciones: async function () {
        const res = await fetch('http://localhost:5082/api/publicaciones');
        const data = await res.json();
        return data;
    },
    // Obtener una publicación por su ID
    getPublicacion: async function (id) {
        const res = await fetch(`http://localhost:5082/api/publicaciones/${id}`);
        const data = await res.json();
        return data;
    },
    // Agregar una nueva publicación
    addPublicacion: async function (publicacion) {
        const res = await fetch('http://localhost:5082/api/publicaciones', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(publicacion)
        });
        const data = await res.json();
        return data;
    },
    // Actualizar una publicación existente por su ID
    updatePublicacion: async function (id, publicacion) {
        const res = await fetch(`http://localhost:5082/api/publicaciones/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(publicacion)
        });
        const data = await res.json();
        return data;
    },
    // Eliminar una publicación por su ID
    deletePublicacion: async function (id) {
        await fetch(`http://localhost:5082/api/publicaciones/${id}`, {
            method: 'DELETE'
        });
        return;
    },
    // Obtener configuraciones, como enumeraciones
    getConfigs: async function () {
        const res = await fetch('http://localhost:5082/api/publicaciones/Configs');
        const data = await res.json();
        return data;
    }
}
