<template>
  <div id="cookouts-list-container">
    <section id="host-actual-section" class="cookouts-section" v-if="$store.state.user.userId != 1">
        <div id="host-section" @click="toggleHostDropDown()">
            <h2 id="hosted-cookouts-h2">Hosted Cookouts</h2>
            <svg id="host-dropdown-arrow-icon" class="dropdown-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M480-345 240-585l43-43 197 198 197-197 43 43-240 239Z"/></svg>
        </div>
        <router-link id="dashboard-create-link" v-bind:to="{name: 'Create-Cookout'}">
            <button  class="create-button">Create a Cookout</button>
        </router-link>
    </section>
    <div class="thin-line" v-if="$store.state.user.userId != 1"></div>
        <transition name="drop-step">       
            <div class="transition-div-host" v-if="this.hostDropdownOpen == true">               
                <div class="card-organizer" v-if="this.hostCookouts.length != 0">
                    <cookout-card class="cookout-card" v-for="cookout in this.hostCookouts" v-bind:cookout="cookout" v-bind:key="cookout.cookoutID"/>
                </div>
                <div class="conditional-display-div" v-else-if="this.hostCookouts.length == 0" >
                    <h2 class="empty-cookout-list-message" v-if="$store.state.user.userId != 1">NO HOSTED COOKOUTS</h2>
                </div>
            </div>
        </transition>
    <section id="invited-section" class="cookouts-section" @click="toggleInvitedDropDown()">
        Invited Cookouts
        <svg id="invited-dropdown-arrow-icon" class="dropdown-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M480-345 240-585l43-43 197 198 197-197 43 43-240 239Z"/></svg>
    </section>
    <div class="thin-line"></div>
    <transition name="drop-step">       
        <div class="transition-div-invited" v-if="this.invitedDropdownOpen == true">
            <div class="card-organizer" v-if="this.invitedCookouts.length != 0">
                <cookout-card class="cookout-card" v-for="cookout in this.invitedCookouts" v-bind:cookout="cookout" v-bind:key="cookout.cookoutID"/>
            </div>
            <div class="conditional-display-div" v-else-if="this.invitedCookouts.length == 0">
                <h2 class="empty-cookout-list-message">NO INVITED COOKOUTS</h2>
            </div>
        </div>
    </transition>
    <section id="chef-section" class="cookouts-section" @click="toggleChefDropDown()" v-if="$store.state.user.userId != 1">
        Chef Cookouts
        <svg id="chef-dropdown-arrow-icon" class="dropdown-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M480-345 240-585l43-43 197 198 197-197 43 43-240 239Z"/></svg>
    </section>
    <div class="thin-line" v-if="$store.state.user.userId != 1"></div>
        <transition name="drop-step">       
            <div class="transition-div-chef" v-if="this.chefDropdownOpen == true">
                <div class="card-organizer" v-if="this.chefCookouts.length != 0">
                    <cookout-card class="cookout-card" v-for="cookout in this.chefCookouts" v-bind:cookout="cookout" v-bind:key="cookout.cookoutID"/>
                </div>
                <div class="conditional-display-div" v-else-if="this.chefCookouts.length == 0">
                    <h2 class="empty-cookout-list-message" v-if="$store.state.user.userId != 1">NO CHEF COOKOUTS</h2>
                </div>
            </div>
        </transition>
  </div>
</template>

<script>
import CookoutService from '../services/CookoutService.js';
import CookoutCard from './CookoutCard.vue';

export default {
    components:{
        CookoutCard
    },
    data() {
        return{
            hostCookouts:[],
            invitedCookouts:[],
            chefCookouts: [],
            hostDropdownOpen: true,
            invitedDropdownOpen:true,
            chefDropdownOpen:true,
        }; 
    },
    methods: {
        retrieveHostCookouts(){
            CookoutService.GetCookoutForHost(this.$store.state.user.userId).then(response => {
                this.hostCookouts = response.data;
            })
        },
        setActiveCookout(){
            this.$store.commit("SET_ACTIVE_COOKOUT",)
        },
        retrieveInvitedCookouts(){
            let thing = !this.$store.state.user.currentEmail ? this.$store.state.user.username : this.$store.state.user.currentEmail
            CookoutService.GetInvitedCookouts(thing).then(response => {
                this.invitedCookouts = response.data;
            })
        },
        retrieveChefCookouts(){
            CookoutService.GetChefCookouts(this.$store.state.user.userId).then(response => {
                this.chefCookouts = response.data;
            })
        },
        toggleHostDropDown(){
            this.hostDropdownOpen = !this.hostDropdownOpen;
            const icon = document.getElementById("host-dropdown-arrow-icon");
            if(this.hostDropdownOpen){
                icon.style.transform = "rotate(0deg)"
            }
            else{
                icon.style.transform = "rotate(-90deg)"
            }
            
        },
        toggleInvitedDropDown(){
            this.invitedDropdownOpen = !this.invitedDropdownOpen;
            const icon = document.getElementById("invited-dropdown-arrow-icon");
            if(this.invitedDropdownOpen){
                icon.style.transform = "rotate(0deg)"
            }
            else{
                icon.style.transform = "rotate(-90deg)"
            }
        },
        toggleChefDropDown(){
            this.chefDropdownOpen = !this.chefDropdownOpen;
            const icon = document.getElementById("chef-dropdown-arrow-icon");
            if(this.chefDropdownOpen){
                icon.style.transform = "rotate(0deg)"
            }
            else{
                icon.style.transform = "rotate(-90deg)"
            }
        },
    },
    created(){
        this.retrieveHostCookouts()
        this.retrieveInvitedCookouts()
        this.retrieveChefCookouts()
    }

};
</script>

<style>

#cookouts-list-container{
    width: 100%;;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: center;
}
.cookouts-section{
    color:#f8f8ff;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    font-size: 2.5em;
    display: flex;
    flex-direction: row;
    align-items: center;
    padding-left: 3%;
    transition: .4s ease;
    width: 100%;
}

#hosted-cookouts-h2{
    font-size: 1em;
    font-weight: normal;
    margin:0;
}

#host-section{
    display: flex;
    justify-content: center;
    cursor: pointer;
    padding-left: 3%;
}

#host-actual-section{
    padding: 0;
}

#invited-section{
    cursor: pointer;
    width: auto;
}

#chef-section{
    cursor: pointer;
    width: auto;
}

.cookout-card{
    min-width: 350px;
    width:20%;
    margin: 2% 2% 1% 2%;
    transition: .4s ease;
}

.cookout-card:hover{
    transform: scale(1.02,1);
    border: 3px solid #f8f8ff;
    transition: .4s ease;
}

.card-organizer{
    display: flex;
    width:100%;
    flex-wrap: wrap;
    justify-content: space-evenly;
    flex-grow: 1;
}

.dropdown-arrow-icon{
    fill: #f8f8ff;
    transition: .3s ease;
}

.conditional-display-div{
    display: flex;
    justify-content: center;
    width:100%;
}

.empty-cookout-list-message{
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:dimgray;
  font-size: 3em;
  text-align: center;
  transition: all .3s ease;
}

.transition-div-host{
    width: 100%;
}
.transition-div-invited{
    width: 100%;
}
.transition-div-chef{
    width: 100%;
}
.transition-div-host #invite-status{
    display: none;
}
.transition-div-chef #invite-status{
    display: none;
}
.thin-line{
    width:100%;
    border-top: 2px solid #f8f8ff;
}

.drop-step-enter,
.drop-step-leave-to { 
  opacity: 0;
  max-height: 0px;
}

.drop-step-leave,
.drop-step-enter-to { 
  opacity: 1;
  max-height: 2000px;
}

.drop-step-enter-active,
.drop-step-leave-active { 
  transition: all 300ms ease;

}

.drop-step-enter div,
.drop-step-leave-to div{ 
  opacity: 0;
  transform: translateY(-5%);
}

.drop-step-leave div,
.drop-step-enter-to div{ 
  opacity: 1;
  transform: translateY(0%);
}

.drop-step-enter-active div,
.drop-step-leave-active div{ 
  transition: all 300ms ease;
}

.create-button{
    margin-right: 3%;
    background-color:#F2AF29 ;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    border: 2px solid #F2AF29;
    width: 160px;
    height: 40px;
    color: #3a3a3b;
    font-size: .4em;
    font-weight: bold;
    cursor: pointer;
    text-decoration: none;
    text-align: center;
    align-self: flex-end;
    transition: .4s ease;
}

#dashboard-create-link{
    text-decoration: none;
    position: absolute;
    right: 0;
    margin-right: 3%;
    margin-bottom: 1%;
}

.create-button:hover{
    border: 2px solid #f8f8ff;
    color:#f8f8ff;
    transition: .4s ease;
}


@media only screen and (max-width: 768px) {
    .create-button{
        display: none;
    }
    .card-organizer{
        margin-bottom: 10%;
    }

}
</style>