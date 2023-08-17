import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});

export default {

    GetCookoutByCookoutID(id) {
        return http.get(`/cookout/${id}`)
    },

    GetAllCookouts() {
        return http.get('/cookout/')
    },

    GetCookoutForHost(id) {
        return http.get(`/cookout/host/${id}`)
    },

    GetCookoutOrders(cookoutId) {
        return http.get(`/cookout/${cookoutId}/orders`)
    },

    CreateCookout(cookout) {
        return http.post(`/cookout`, cookout)
    },

    UpdateCookout(cookout) {
        return http.put(`/cookout/${cookout.cookoutId}`, cookout)
    },

    GetInvitedCookouts(email) {
        return http.get(`/cookout/guest/${email}`)
    },
    GetChefCookouts(id) {
        return http.get(`/cookout/chef/${id}`)
    },
    GetCookoutMenu(id){
        return http.get(`/cookout/${id}/menu`)
    },





}