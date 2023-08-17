import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});
export default {

    GetMenuItemById(id){
        return http.get(`/menu/${id}`)
    },

    GetMenu(){
        return http.get('/menu')
    },

    CreateMenuItem(item){
        return http.post('/menu/', item)
    },

    UpdateMenuItem(item){
        return http.put(`/menu/${item.item_id}`, item)
    }
    


}
