<template>
<div class="rsvp-container">
  <h1>{{invite.personName}}, Please RSVP for {{cookout.cookoutName}}</h1>
  <h3>Are you able to come to this event?</h3>
  <div class="rsvp-form">
    <div id="rsvp-radio-container">
      <div class="rsvp-choice">
        <label for="yes">Yes, I am able to come</label>
        <input type="radio" id="yes" name="rsvp" v-on:click="canCome = true">
      </div>
      <div class="rsvp-choice">
      <label for="no">No, I am not able to attend</label>
      <input type="radio" id="no" name="rsvp" v-on:click="canCome = false">
      </div>
    </div>
    <div class="name-change">
      <p>If you would like to be referred as a different name, please input below:</p>
      <input type="text" v-bind:placeholder="invite.personName" v-model="invite.personName">
    </div>
    <div class="add-menu-item">
      
      <button id="open-menu-form" v-on:click="isFormVisible = !isFormVisible">Add Additional Goodie</button>
      <transition name="slide">
        <menu-item-creation @addItem="addSelectedItems" v-show="isFormVisible === true"></menu-item-creation>
      </transition>
    </div>
    <div>
      <button id="submit-rsvp-btn" type="submit" v-on:click="submitReply()">Submit</button>
    </div>
  </div>
</div>

</template>

<script>
import CookoutService from '../services/CookoutService'
import InviteService from '../services/InviteService.js'
import MenuService from '../services/MenuService';
import MenuItemCreation from './MenuItemCreation.vue';
export default {
  components: {MenuItemCreation},
  data() {
    return {
      canCome: false,

      currentInvite: {},
      currentCookout: {},
      isFormVisible: false,
      
    }
  },
  methods: {
    retreiveCookout() {
      CookoutService.GetCookoutByCookoutID(this.$route.params.id).then(response => {
        this.$store.commit("SET_ACTIVE_COOKOUT", response.data)
        this.currentCookout = response.data;
      }).catch(error => {
        if(error.response && error.response.status === 404){
          alert("Cookout not available. This cookout may have been deleted or you have entered an invalid cookout ID.");
        }
      });
    },
    retrieveInvite() {
      this.currentInvite = InviteService.GetInviteById(this.$route.params.inviteId).then(response => {
        this.$store.commit("SET_ACTIVE_INVITE", response.data)
        this.currentInvite = response.data;
      }).catch(error => {
        if(error.response && error.response.status === 404){
          alert("You were not invited by the host. Please contact them for further information.");
        }
      });
    },
    submitReply() {
      if(this.canCome === true){
        this.currentInvite.responseStatus = 1;
        this.currentCookout.numberOfAttendees++;
        alert("Thanks for your reply. We can't wait to see you!");
        this.$router.push({name: 'CookoutDetails', params: {id: this.$route.params.id}});
      }
      else{
        this.currentInvite.responseStatus = 2;
        alert("Thanks for your reply!");
        this.$router.push('/');
      }
      CookoutService.UpdateCookout(this.currentCookout);
      InviteService.UpdateInvite(this.currentInvite);
    },
    addSelectedItems(item) {
      MenuService.CreateMenuItem(item);
    }
  },
  created(){
      this.retreiveCookout();
      this.retrieveInvite();
  },
  computed:{
    cookout() {
      return this.$store.state.activeCookout;
    },
    invite() {
      return this.$store.state.activeInvite;
    }
  }
}
</script>

<style>

.rsvp-container{
  display: flex;
  flex-direction: column;
  width: 100%;
  align-items: center;
  justify-content: center;
}

.rsvp-container h3{
  color: #f8f8ff;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 2em;
  margin-left: 3%;
  margin-right: 3%;
  text-align: center;
}

.rsvp-form{
  width: 95%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

#rsvp-radio-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.rsvp-choice{
  display: flex;
  align-items: center;
  justify-content: space-between;
  min-width: 350px;
  margin-bottom: 4%;
}

.rsvp-choice label{
  color: #f8f8ff;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.4em;
  color: #F2AF29;
}

#submit-rsvp-btn{
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 3%;
  min-width: 200px;
  height:40px;
  margin-top: 10%;
}
.name-change{
  color: whitesmoke;
  font-size: 16pt;
  text-align: center;
}

.name-change p {
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
.add-menu-item{
  display: flex;
  flex-direction: column;
}
#open-menu-form{
  text-align: center;
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 3%;
  min-width: 200px;
  height:40px;
  margin-top: 10%;
}

</style>