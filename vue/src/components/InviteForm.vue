<template>
<div class="form">
  <router-link class="details-btn-link" v-bind:to="{name: 'CookoutDetails', params:{id: this.$route.params.id}}">
    <div class="back-to-details-btn">
      <svg class="details-back-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m274-450 248 248-42 42-320-320 320-320 42 42-248 248h526v60H274Z"/></svg>
      <p >Back to Details</p>
    </div>
  </router-link>
  <div class="title">
    <h1>Send invites</h1>
  </div>
  <div class="select-segment">
    <h3>Select an existing user</h3>
    <form v-on:submit.prevent>
      <select id="existing-users" name="dropdown" v-model="invite.inviteEmail">
        <option></option>
        <option v-for="hedgehog in $store.state.users" v-bind:key="hedgehog.userId">{{hedgehog.username}}</option>
      </select>
      <h3>What is their email</h3>
        <input type="text" class="text-input" v-model="invite.inviteEmail">
        <h3>What is their name?</h3>
        <input type="text" class="text-input" v-model="invite.personName">
        <br/>
        <br/>
        <button type="submit" class="save-invites" v-on:click="saveInvite(invite)">Save invite</button>
    </form>
  </div>
  <div class="invite-display">
    <form class="invites-to-send" v-on:submit.prevent>
      <ul v-if="$store.state.invites.length != 0">
        <li v-for="giraffe in $store.state.invites" v-bind:key="giraffe.inviteId">
          <input type="radio" id="checkId" v-on:click="populateInvites(giraffe)">{{giraffe.personName}}</li>
      </ul>
      <div class="invite-buttons">
        <button type="submit" class="send" v-on:click="sendInvites()">Send Invites</button>
        <button class="remove" v-on:click.prevent="removeInvites()">Remove Invites</button>
      </div>
    </form>
  </div>
  <div class="view">
    <button class="view-invites" v-show="viewInvites === false" v-on:click="getInvitesByCookout($route.params.id), viewInvites = ! viewInvites">View Invite Status</button>
    <div class="invites-sent">
      <button class="hide-invites" v-show="viewInvites === true" v-on:click="viewInvites = !viewInvites">Hide Invites</button>
        <table v-show="viewInvites === true" id="invites-table"> 
          <tr class="invites-titles">
            <th id="title-name" @click="sort('name')" >Name</th>
            <th id="title-email" @click="sort('email')">E-Mail</th>
            <th id="title-response" @click="sort('response')">Response</th>
          </tr>
          <tr v-for="i in $store.state.invitesToShow" v-bind:key="i.inviteId" class="invites-information">
            <td class="data-name">{{i.personName}}</td>
            <td class="data-email">{{i.inviteEmail}}</td>
            <td class="pill">
              <span id="pill" v-bind:style="i.responseStatus === 1 ? { 'background-color': 'rgb(167, 243, 208)', 'color': 'rgb(6, 95, 70)' }: 
              i.responseStatus === 2 ? { 'background-color': 'salmon', 'color': 'rgb(153, 27, 54)' }: { 'background-color': 'rgb(253, 230, 138)', 
              'color': 'rgb(146, 64, 14)' } ">{{convertResponse(i.responseStatus)}}</span>
            </td>
          </tr>
        </table>
    </div>
    <button class="hide-invites" v-show="viewInvites === true" v-on:click="viewInvites = !viewInvites">Hide Invites</button>
  </div>
</div>
</template>

<script>
import inviteService from "../services/InviteService.js";
import userService from "../services/UserService.js";
import cookoutService from "../services/CookoutService.js";
export default {
  data() {
    return {
      invite: {
        //inviteId: 0,
        inviteEmail: "",
        personName: "",
        isSent: false,
        cookoutId: Number(this.$route.params.id),
        responseStatus: 3,
      },
      user: {
        userId: 0,
        username: "",
        name: "",
      },
      selectedInvites: [],
      viewInvites: false,
      currentCookout: {},
      search : {
        name: '',
        email: '',
        response: '',
      }
    }
  },
  methods: {
    getInvites() {
      inviteService.GetAllInvites().then((response) => {
        this.$store.commit("SET_INVITES", response.data);
      });
    },
    getInvitesByCookout(id) {
      inviteService.GetInvitesByCookoutId(id).then((response) => {
        response.data.sort((a, b) => a.personName.localeCompare(b.personName));
        this.$store.commit("SET_INVITES_TO_SHOW", response.data);
      });
    },
    retrieveActiveCookout() {
      cookoutService.GetCookoutByCookoutID(this.invite.cookoutId).then((response) => {
        this.$store.commit("SET_ACTIVE_COOKOUT", response.data);
      });
    },
    getUsers() {
      userService.GetAllUsers().then((response) => {
        this.$store.commit("SET_USERS", response.data);
      });
    },
    saveInvite(invite) {
      this.$store.commit("ADD_INVITE", invite);
      this.invite = {
        inviteId: 0,
        inviteEmail: "",
        personName: "",
        isSent: false,
        cookoutId: Number(this.$route.params.id),
        responseStatus: 3,
      };
    },
    convertResponse(response) {
      if(response == 1){
        return "Response: Yes";
      }
      if(response == 2){
        return "Response: No";
      }
      if(response == 3){
        return "Response: Pending";
      }
    },
    populateInvites(invite) {
        this.selectedInvites.push(invite);
    },
    sendInvites() {
      this.selectedInvites.forEach((invite) => {
        invite.isSent = true;
        this.selectedInvites = [];
        this.$store.state.invites = [];
        inviteService.CreateInvite(invite)
      });
    },
    removeInvites() {
      this.selectedInvites.forEach((invite) => {
        this.$store.commit("REMOVE_INVITE", invite);
        this.selectedInvites = [];
        document.getElementById('checkId').checked = false;
      })
    }
  },
  created() {
    this.getUsers();
    this.retrieveActiveCookout();
  },
  computed: {
    cookout() {
      return this.$store.state.activeCookout;
    },
    filteredList() {
      let list = this.user;

      if(this.search.name != ""){
        list = list.filter((user) =>{
          return user.name.toLowerCase().includes(this.search.name.toLowerCase());
        });
      }
        if(this.search.email != ""){
          list = list.filter((user) =>{
            return user.email.toLowerCase().includes(this.search.email.toLowerCase());
          });
        }
          if(this.search.response != ""){
            list = list.filter((user)=>{
              return user.response.includes(this.search.response);
            })
          }
          return list
        }
      }
    
  
}
</script>

<style>
.title h1 {
  text-align: center;
  font-family: Impact, Haettenschweiler, "Arial Narrow", sans-serif;
  color: #ffc000;
  font-weight: 100;
  font-size: 40pt;
}
#existing-users {
  font-size: 1.2em;
}
.select-segment{
  display: flex;
  flex-direction: column;
  text-align: center;
}
.select-segment h3{
  color: #F2AF29;
}
.text-input{
  width: 325px;
}
.invites-to-send{
  text-align: center;
  color: #f2af29;
}
.invites-to-send li {
  list-style-type: none;
}
.save-invites{
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 2%;
  min-width: 200px;
  height:40px;
}
.invite-buttons button{
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 3%;
  min-width: 150px;
  margin-left: 7.5px;
  margin-right: 7.5px;
  height:40px;
}
.view{
  text-align: center;
}
.view-invites{
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  height:40px;
  min-width: 200px;
  text-align: center;
}
.hide-invites{
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  height:40px;
  min-width: 200px;
  text-align: center;
  margin-bottom: 15px;
  margin-top: 15px;
}
.invites-sent{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
#invites-table{
  width: 80%;
  color: whitesmoke;
  text-align: center;
  align-content: center;
}
#invites-titles{
  text-align: center;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#invites-table tr th{
  height: 35px;
  border-bottom: 3px solid lightgrey;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#invites-table tr td{
  height: 35px;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}

.select-segment h3{
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#pill {
  color:black;
  display: inline-block;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 1rem;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
  #title-name{
    width: 33%;
  }
  #title-email{
    width: 33%;
  }
  #title-status{
    width: 33%
  }
  .data-name{
    width: 33%;
  }
  .data-email{
    width: 33%;
  }
  .pill{
    width: 33%;
  }
@media only screen and (max-width: 768px) {
  #invites-table{
    width: 100%;
    font-size: .8em;
  }
  #title-name{
    width: 20%;
  }
  #title-email{
    width: 50%;
  }
  #title-status{
    width: 30%
  }
  .data-name{
    width: 20%;
  }
  .data-email{
    width: 50%;
  }
  .pill{
    width: 30%;
  }
  .invites-titles{
    width: 100%;
  }
  .invites-information{
    width: 100%;
  }
}
</style>