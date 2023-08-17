<template>
  <div id="create-cookout-container">
    <div id="create-back-button">
    <router-link id="dashboard-btn-link" v-bind:to="{name: 'Cookouts'}">
      <div class="back-to-details-btn">
        <svg class="details-back-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m274-450 248 248-42 42-320-320 320-320 42 42-248 248h526v60H274Z"/></svg>
        <p>Back to Dashboard</p>
      </div>
    </router-link>
    </div>
    <h1>Create a Cookout</h1>
    <form id="cookout-form" v-on:submit.prevent>
      <div class="form-element">
        <label for="name"> Name of Cookout </label>
        <input type="text" id="name" v-model="cookout.cookoutName"/>
      </div>
      <div class="form-element">
        <label for="time"> Date & Time </label>
        <input type="datetime-local" id="time" v-model="cookout.dateOfEvent"/>
      </div>
      <div class="form-element">
        <label for="chef"> Chef </label>
        <select id="existing-chefs" name="dropdown" v-model="cookout.chefId" >
          <option></option>
          <option v-for="user in this.filteredUsers" :value="user.username" v-bind:key="user.userId">{{user.name}} ---- {{user.username}}</option>
        </select>
      </div>
      <div class="form-element address-field">
        <label for="address"> Please Enter Address </label>
        <input type="text" placeholder="Address Line" id="address" v-model="cookout.location.streetAddress"/>
      </div>
    <div id="city-state-zip">
      <div id="city-container"  class="form-element address-field">
        <label for="city">City </label>
        <input type="text" placeholder="City" id="city" v-model="cookout.location.city"/>
      </div>
      <div id="state-container"  class="form-element address-field">
        <label for="state">State</label>
        <select id="state" name="dropdown" v-model="cookout.location.stateAbbreviation">
          <option v-for="item in $store.state.stateAbbrevs" v-bind:key="item">{{item}}</option>
        </select>
      </div>
      <div id="zipcode-container" class="form-element address-field">
        <label for="zip">Zip</label>
        <input type="text" placeholder="ZIP" id="zip" v-model="cookout.location.zipcode"/>
      </div>
    </div>
    <div id="description" class="form-element description">
        <label for="description"> Description of Event </label>
        <textarea id="description" v-model="cookout.description"/>
      </div>
      <div class="submit">
        <button id="create-cookout-btn" class="btn btn-submit" type="submit" v-on:click="submitCookout()">Create Cookout!</button>
      </div>
    </form>
  </div>
</template>

<script>
import cookoutService from '../services/CookoutService'
import UserService from '../services/UserService'
export default {
  name: "create-cookout",
  props: {
    cookoutId: {
      type: Number,
      default: 0
    }
  },
  data() {
    return {
      cookout:{
        cookoutID: 0,
        hostId: this.$store.state.user.userId,
        chefId: '',
        cookoutName: '',
        dateOfEvent: '',
        location: {
          streetAddress: '',
          city:'',
          stateAbbreviation: '',
          zipcode: '',
        },
        description: '',
      },
      users: []
    }
 },
 methods: {
   submitCookout() {
     let c = this.getChefId()
     const newCookout = {
       cookoutID: Number(this.cookout.cookoutID),
       hostId: Number(this.cookout.hostId),
       cookoutName: this.cookout.cookoutName,
       chefId: Number(c),
       dateOfEvent: this.cookout.dateOfEvent,
       location: {
         streetAddress: this.cookout.location.streetAddress,
         city: this.cookout.location.city,
         stateAbbreviation: this.cookout.location.stateAbbreviation,
         zipcode: Number(this.cookout.location.zipcode)
       },
       description: this.cookout.description
     }

     cookoutService.CreateCookout(newCookout).then(response => {
       if (response.status === 201) {
         this.$router.push({name: 'CookoutDetails', params: {id: response.data.cookoutID}});
       }
     }) .catch(error => {
       this.handleErrorResponse(error,'creating');
     });
   },
   handleErrorResponse(error, verb) {
      if (error.response) {
        this.errorMsg =
          "Error " + verb + " card. Response received was '" +
          error.response.statusText +
          "'.";
      } else if (error.request) {
        this.errorMsg =
          "Error " + verb + " card. Server could not be reached.";
      } else {
        this.errorMsg =
          "Error " + verb + " card. Request could not be created.";
      }
    },
    getUsers(){
      UserService.GetAllUsers().then(response =>{
       let x =  response.data.sort((a, b) => a.name.localeCompare(b.name));
        this.users = x
      });
    },
    getChefId(){
      let findChef = this.filteredUsers
      findChef.filter(chef => {
        return chef.username === this.cookout.chefId
      });
      this.cookout.acutalChefId = findChef[0].userId
      return findChef[0].userId;
    }
  },
  created(){
    this.getUsers()
  },
  computed:{
    filteredUsers(){
     return this.users.filter(item => {
       return item.name != "admin" && item.name != "user"
      })
    },
  }
 
}
</script>

<style>


#create-back-button{
  align-self: flex-start;
  margin-top: 2%;
  margin-left: 5%;
}
#create-cookout-container{
  display:flex;
  flex-direction: column;
  align-items:center;
  justify-content: center;
  width: 100%;
}

#cookout-form{
  display:flex;
  flex-direction: column;
  width:95%;
  margin-right:2.5%;
  margin-left: 2.5%;
  align-items: center;
  justify-content: center;
  margin-bottom: 5%;
}

#cookout-form > .form-element{
  display: flex;
  flex-direction: column;
  width:50%;
}
.form-element{
  color:#F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  margin-bottom: 2%;
}

.form-element label{
  margin-bottom: 1%;
  font-size: 1.2em;
  text-align: center;
}

#create-cookout-btn{
  height:40px;
  width: 130px;
  border: 2px solid #F2AF29;
  background-color: #F2AF29;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  transition: .3s ease;
}

#create-cookout-btn:hover{
  color: #f8f8ff;
  border: 2px solid #f8f8ff;
  transition: .3s ease;
}

#cookout-form .submit{
  display: flex;
  align-items: center;
  justify-content: center;
}

#city-state-zip{
  display: flex;
  flex-direction: row;
  flex-wrap: nowrap;
  justify-content: center;
  align-items: center;
  width:30%;
  margin-bottom: 1%;
}

#city-state-zip .form-element{
  display: flex;
  flex-direction: column;

}

#city-container{
  width:60%;
}
#state-container{
  width: 10%;
  margin-left: 5%;
  margin-right: 5%;
}
#zipcode-container{
  width: 20%;
}

#cookout-form select,input,option{
  height: 30px;
  font-size: 1.1em;
}

#description{
  height: 150px;
  margin-bottom: 4%;
}

#cookout-form > div{
  min-width: 300px;
}

#back-to-dashboard-btn{
  display: flex;
  width: 240px;
  height: 60px;
  align-items: center;
  margin-top: 2%;
  margin-left: 5%;
  text-align: left;
  background-color: #F2AF29;
  padding: 10px;
  color: #3a3a3b;
  font-family: Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
  text-decoration: none;
  font-size: 1.3em;
  transition: .3s ease-in;
  border: 2px solid #F2AF29;
  align-self: flex-start;
}

#back-to-dashboard-btn:hover{
  background-color: #F2AF29;
  color: #f8f8ff;
  border: 2px solid #f8f8ff;
  text-decoration: underline;
  transition: .3s ease-in;
}
#dashboard-back-arrow-icon{
  fill: #3a3a3b;
  transition: .3s ease-in-out;
}

 #back-to-dashboard-btn:hover #dashboard-back-arrow-icon{
  fill: #f8f8ff;
  transition: .3s ease-in;
}


#dashboard-btn-link{
  text-decoration: none;
}

</style>