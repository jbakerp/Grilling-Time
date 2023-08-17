import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});
export default {

    GetOrderByOrderId(cookoutId, id){
        return http.get(`/orders/cookout/${cookoutId}/orders/${id}`)
    },
    GetItemsByOrderId(id, cookoutId){
        return http.get(`/orders/cookout/${cookoutId}/orders/${id}/items`)
    },

    UpdateOrder(order, cookoutId){
        return http.put(`/orders/cookout/${cookoutId}/orders/${order.orderId}`, order)
    },
    CreateOrder(cookoutId, order){
        return http.post(`/orders/cookout/${cookoutId}/orders`, order)
    },

    

}