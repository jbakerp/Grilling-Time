<template>
  <div class='order-list-conatiner'>
    <div id="snackbar">Order Finished!</div>
    <router-link class="details-btn-link" v-bind:to="{name: 'CookoutDetails', params:{id: this.$route.params.id}}">
    <div class="back-to-details-btn">
      <svg class="details-back-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m274-450 248 248-42 42-320-320 320-320 42 42-248 248h526v60H274Z"/></svg>
      <p >Back to Details</p>
    </div>
    </router-link>
    <h1 id="order-list-title">{{$store.state.activeCookout.cookoutName}} Order List</h1>
    <ul class="order-list" v-if="orderCount > 0">
      <order-card  v-for="order in orderList" @order-complete="showSnackBar" v-bind:key="order.order_id" v-bind:order="order" />
    </ul>
    <h2 id="empty-order-list-text" v-else>THERE ARE NO PENDING ORDERS!</h2>
  </div>
</template>

<script>
import CookoutService from '../services/CookoutService.js'
import OrderCard from './OrderCard.vue'

export default {
  components: {OrderCard},
  data() {
    return {
      orderList:[],
      itemList: []
    }
  },
  methods: {
    retrieveCookoutOrders(){
      CookoutService.GetCookoutOrders(this.$route.params.id).then(response => {
        this.orderList = response.data.filter(item =>{
          return item.isCompleted === false;
        })
      })
    },
    setActiveCookout(){
      CookoutService.GetCookoutByCookoutID(this.$route.params.id).then(response => {
        this.$store.commit("SET_ACTIVE_COOKOUT", response.data);
      })
    },
    showSnackBar(){
      var x = document.getElementById("snackbar");
      x.className = "show";
      setTimeout(function(){x.className = x.className.replace("show","");},3000);

    },    
  },
  created(){
    this.retrieveCookoutOrders();
    this.setActiveCookout();
  },
  computed: {
    orderCount(){
      return this.orderList.length
    }
  }
  
}
</script>

<style>
.order-list-container{
  width: 100%;
  height: 100%;
}

.order-list{
  width: 100%;
  height:100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  padding: 0;
  margin-bottom: 3%;
}

#order-list-title{
  margin-bottom: 5%;
}

.food-order{
  border: 3px solid #f8f8ff;
  width: 50%;
  height:200px;
  list-style: none;
  margin-bottom: 20px;
  border-radius: 12px;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:#f8f8ff;
  font-size: 1.7em;
  background-color: #f37d42;
  display: flex;
  justify-content: center;
  align-items: center;
}

.order-header{
  text-align: center;
}

#empty-order-list-text{
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:dimgray;
  font-size: 3em;
  text-align: center;
}

.back-to-details-btn{
  display: flex;
  width: 240px;
  height: 60px;
  justify-content: center;
  align-items: center;
  margin-top: 2%;
  margin-left: 5%;
  background-color: #F2AF29;
  padding: 10px;
  color: #3a3a3b;
  font-family: Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
  text-decoration: none;
  font-size: 1.3em;
  transition: .3s ease-in;
  border: 2px solid #F2AF29;
}

.back-to-details-btn:hover{
  background-color: #F2AF29;
  color: #f8f8ff;
  border: 2px solid #f8f8ff;
  font-family: Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
  text-decoration: underline;
  transition: .3s ease-in;
}
.details-back-arrow-icon{
  fill: #3a3a3b;
  transition: .3s ease-in-out;
}

 .back-to-details-btn:hover .details-back-arrow-icon{
  fill: #f8f8ff;
  transition: .3s ease-in;
}

.details-btn-link{
  text-decoration: none;
}
#snackbar {
  visibility: hidden;
  min-width: 250px;
  margin-left: -125px;
  background-color: #333;
  color: lightgreen;
  text-align: center;
  border-radius: 2px;
  padding: 16px;
  position: fixed;
  z-index: 30;
  left: 50%;
  top:100px;
  font-size: 22px;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}

#snackbar.show {
  visibility: visible;
  -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
  animation: fadein 0.5s, fadeout 0.5s 2.5s;
}

@-webkit-keyframes fadein {
  from {top: 0; opacity: 0;} 
  to {top: 100px; opacity: 1;}
}

@keyframes fadein {
  from {top: 0; opacity: 0;}
  to {top: 100px; opacity: 1;}
}

@-webkit-keyframes fadeout {
  from {top: 100px; opacity: 1;} 
  to {top: 0; opacity: 0;}
}

@keyframes fadeout {
  from {top: 100px; opacity: 1;}
  to {top: 0; opacity: 0;}
}


</style>