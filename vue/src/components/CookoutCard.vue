<template>
<router-link class="cookout-card-links" v-bind:to="{ name: 'CookoutDetails', params: {id: cookout.cookoutID} }">
  <div class="cookout-card-container">
      <div class="cookout-card-general">        
        <h2 class="cookout-card-title">{{cookout.cookoutName}}</h2>
        <div class="cookout-bubble">
            <p class="attendance"> Guests: {{cookout.numberOfAttendees}}</p>
            <p class="attendance" id="invite-status" 
            v-bind:style="invite.responseStatus === 1 ? { 'color': 'rgb(167, 243, 208)' }: 
              invite.responseStatus === 2 ? { 'color': 'salmon' }: { 'color': 'rgb(253, 230, 138)' } ">{{convertResponse(invite.responseStatus)}}</p>
        </div>
        <p class="cookout-card-description">{{cookout.description}}</p>
        <p>{{formatDate(cookout.dateOfEvent)}}</p>

      </div>
      <div class="cookout-card-address">
          <p>{{cookout.location.streetAddress}}</p>
          <div class="address-sub-container">
            <p>{{cookout.location.city}}, {{cookout.location.stateAbbreviation}} {{cookout.location.zipcode}}</p>
          </div>
      </div>
  </div>
  </router-link>
</template>

<script>
import moment from 'moment'
import InviteService from '../services/InviteService.js';
export default {
    props: ['cookout'],
    data () {
        return {
            invite: {},
        }
    },
    methods: {
        formatDate(value){
            if(value){
                return moment(String(value)).format('LLLL')
            }
        },
        getInvitesForUser(){
                let email = this.$store.state.user.username == 'user@gmail.com' ? this.$store.state.user.currentEmail : this.$store.state.user.username;
                if(this.$store.state.user.userId != this.cookout.hostId && this.$store.state.user.userId != this.cookout.chefId){
                InviteService.GetInviteByEmailAddress(this.cookout.cookoutID, email).then(response => {
                    this.invite = response.data;
                })
                }
        },
        convertResponse(response) {
            if(response == 1){
                return "Coming";
            }
            if(response == 2){
                return "Not Going";
            }
            if(response == 3){
                return "Pending";
            }
        },
    },
    created(){
        this.getInvitesForUser();
    }
}
</script>

<style>

.cookout-card-container{
    height: 100%;
    width: 100%;
    border-radius: 12px;
}

.cookout-card-links:nth-child(1n){
    background-color: #f37d42;
    border-radius: 12px;
}

.cookout-card-links:nth-child(2n){
    background-color: #F2AF29;
    border-radius: 12px;
}
.cookout-card-links:nth-child(3n){
    background-color: #0a8ae0e0;
    border-radius: 12px;
}
.cookout-card-links:nth-child(4n){
    background-color: #f04c46c5;
    border-radius: 12px;
}
.cookout-card-links:nth-child(5n){
    background-color: #40da19c5;
    border-radius: 12px;
}
.cookout-card-links:nth-child(6n){
    background-color: #cf55c5c5;
    border-radius: 12px;
}


.cookout-card-general{
    display: flex;
    flex-direction: column;
    text-align: center;
    align-items: center;
    justify-content: center;
}
.cookout-card-general h2{
    margin-bottom: 0;
    text-shadow: rgba(black, 0.5) 0 10px 10px;
}


.cookout-card-general p{
    margin-left: 3%;
    margin-right: 3%;
    margin-bottom: 0;
}

.cookout-card-container p{
    font-family: 'Fugaz One', 'Arial Narrow', Arial, sans-serif;  
}
.cookout-card-title{
    color:#f8f8ff;
    font-family: 'Courgette', 'Arial Narrow', Arial, sans-serif;
    justify-self: center;
    margin-left: 1%;
    margin-right: 1%;
}

.cookout-card-address{
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-end;
    margin-top: 5%;
    text-align: center;
    margin-bottom: 3%;
}
.cookout-card-address p{
    width: 100%;
    margin: 0;
}
.address-sub-container{
    display: flex;
    width: 100%;
    justify-content: space-evenly;
}
.cookout-bubble{
    display:flex;
    width: 100%;
    justify-content: center;
}
.attendance{
    display: flex;
    background-color: #3a3a3b;
    border-radius: 16px;
    color:#f8f8ff;
    min-width: 100px;
    min-height: 40px;
    max-width: 100px;
    max-height: 40px;
    text-align: center;
    align-items: center;
    justify-content: center;
    justify-self: flex-start;
    margin-left: 3%;
    font-weight: bolder;
}

.cookout-card-links{
    text-decoration:none;
    color: inherit;
}


</style>