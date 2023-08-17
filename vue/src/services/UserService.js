import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});

export default {

    GetAllUsers() {
        return http.get(`/user`);
    },
    GetUserByUsername(username){
        return http.get(`/user/username/${username}`)
    }
}