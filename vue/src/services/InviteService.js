import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});

export default {

    GetAllInvites() {
        return http.get(`/invite`);
    },
    GetAllInvitesByEmail(email) {
        return http.get(`/invite/email/${email}`);
    },
    GetInviteByEmailAddress(cookoutId, email){
        return http.get(`invite/${cookoutId}/email/${email}`);
    },
    GetInviteById(id){
        return http.get(`/invite/${id}`);
    },
    GetInvitesByCookoutId(cookoutId){
        return http.get(`/invite/cookout/${cookoutId}`);
    },
    UpdateInvite(invite){
        return http.put(`/invite/${invite.inviteId}`, invite);
    }, 
    CreateInvite(invite){
        return http.post(`/invite`, invite);
    }
}