<template>
  <div id="cookout-details-container">
    <div id="create-back-button">
    <router-link id="dashboard-btn-link" v-bind:to="{name: 'Cookouts'}">
      <div class="back-to-details-btn">
        <svg class="details-back-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m274-450 248 248-42 42-320-320 320-320 42 42-248 248h526v60H274Z"/></svg>
        <p>Back to Dashboard</p>
      </div>
    </router-link>
    </div>
    <h1 id="cookout-name">{{ cookout.cookoutName }}</h1>
    <h2 class="cookout-general-deatils">{{ formatDate(cookout.dateOfEvent) }}</h2>
    <h2 class="cookout-general-deatils">{{ cookout.description }}</h2>
    <h3 class="cookout-general-deatils">{{ cookout.location.streetAddress }}</h3>
    <h3 class="cookout-general-deatils">{{cookout.location.city}}, {{cookout.location.stateAbbreviation}} {{cookout.location.zipcode}}</h3>
    
    <a id="directions-link" :href="this.linkCreator()"> Directions</a>
    <div id="details-btns-container">
        <button @click="toggleMenu" id="menu-btn">Menu</button>
        <router-link v-bind:to="{name: 'rsvp', params:{id: this.$route.params.id, inviteId: $store.state.activeInvite.inviteId}}">
          <button id="rsvp-btn" v-if="this.$store.state.activeCookout.hostId !== this.$store.state.user.userId"
          v-on:click="retrieveInvite">RSVP</button>
        </router-link>
        <router-link v-bind:to="{name: 'ShortOrderList', params:{id: cookout.cookoutID}}" v-if="$store.state.user.userId == cookout.chefId">
          <button id="orders-list-btn"> Orders</button>
        </router-link>
      <router-link id="invite-link" v-bind:to="{name: 'Invites', params: {id: cookout.cookoutID}}" v-if="$store.state.user.userId == cookout.hostId"> 
        <button id="manage-invites-btn">Manage Invites</button>
      </router-link> 
    </div>
    <transition name="slide">
      <menu-list v-show="this.menuVisible == true"/>
    </transition>
  </div>
</template>

<script>
import CookoutService from "../services/CookoutService";
import moment from "moment";
import MenuList from './MenuList.vue';
import InviteService from '../services/InviteService.js';

export default {
  components: { MenuList },
  data() {
    return {
      menuVisible: false,
      mapLink: "",
      selectedItem: null,
      order: [],
      thisCookout: {},
    }
  },
  methods: {
    retreiveCookout() {
      CookoutService.GetCookoutByCookoutID(this.$route.params.id)
        .then((response) => {
          this.$store.commit("SET_ACTIVE_COOKOUT", response.data);
          this.thisCookout = response.data;
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            alert(
              "Cookout not available. This cookout may have been deleted or you have entered an invalid cookout ID."
            );
          }
        });
      },
    retrieveInvite() {
        let email = !this.$store.state.user.currentEmail ? this.$store.state.user.username : this.$store.state.user.currentEmail;
          InviteService.GetInviteByEmailAddress(this.$route.params.id, email)
            .then((response) => {
              this.$store.commit("SET_ACTIVE_INVITE", response.data);
            });
      },
    formatDate(value) {
      if (value) {
        return moment(String(value)).format("LLLL");
      }
    },
    toggleMenu(){
      this.menuVisible = !this.menuVisible;
    },
    linkCreator() {
      let result ="https://www.google.com/maps/place/";
      let street = this.$store.state.activeCookout.location.streetAddress.split(" ")
      result += String(street[0]) + "+"
      result += String(street[1]) + "+"
      result += street[2] + ",+"
      result += this.$store.state.activeCookout.location.city + ",+"
      result += this.$store.state.activeCookout.location.stateAbbreviation + "+"
      result += this.$store.state.activeCookout.location.zipcode
      this.mapLink = result;
      return result;
    }
  },
  created() {
    this.retreiveCookout();
    this.linkCreator();
    this.retrieveInvite();

  },
  computed: {
    cookout() {
      return this.$store.state.activeCookout;
    },
    
    
  },
};
</script>


<style>

#cookout-details-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin-bottom: 5%;
  height: 100%;
  text-align: center;
}
#cookout-details-container #cookout-name {
  font-style: oblique;
  font-family: 'Righteous';
}
.cookout-general-deatils {
  color: #f2af29;
  font-family: "Righteous";
}
#cookout-details-container h3 {
  color: #f2af29;
  font-family: "Righteous";
  font-weight: bolder;
}


#details-btns-container{
  display:flex;
  flex-direction: row;
  align-content: space-between;
  justify-content: center;
  width: 100%;

}

#details-btns-container button{
  min-width:100px;
  min-height: 40px;
  background-color: #F2AF29;
  border: 2px solid #F2AF29;
  color:#3a3a3b;
  font-size: 1em;
  font-weight: bold;
  cursor:pointer;
  transition: .3s ease;
  margin-left: 10px;
  margin-right: 10px;
}

#details-btns-container button:hover{
  border:2px solid #f8f8ff;
  background-color: #fdba34;
  color: #f8f8ff;
  transition: .3s ease;
}

#directions-link{
  text-decoration: underline;
  font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color: #f8f8ff;
  font-size: 1.3em;
  margin-bottom: 2%;
  margin-top: 2%;
}

.slide-enter,
.slide-leave-to { 
  opacity: 0;
  max-height: 0;
  transform: translateY(-10%);
}

.slide-leave,
.slide-enter-to { 
  opacity: 1 ;
  transform: translateY(0%);
  max-height: 4000px;
}

.slide-enter-active,
.slide-leave-active { 
  transition: all 350ms;
}

#map-location{
  width:400px;
  height:400px;
}

</style>