<template>
  <div class="menu-list">
    <h1 id="menu-list-title"> Menu for {{this.$store.state.activeCookout.cookoutName}}</h1>
    <router-link id=menu-setup-link v-bind:to="{name: 'setMenu', params:{id: this.$route.params.id}}" v-if="$store.state.user.userId === $store.state.activeCookout.hostId">
      <button id="menu-setup">Set-up Menu</button>
    </router-link>
    <div id="order-snack">Added to order!</div>
      <div class="foodcard">
        <food-card v-for="menuItem in this.menu" @addItemToOrder="addItem(menuItem), showSnackBug()" v-bind:menuItem="menuItem" v-bind:key="menuItem.itemID"/>
      </div>
    <transition name="drop-step"> 
      <cart v-if="this.$store.state.cart.length > 0"/>
    </transition>
  </div>
</template>

<script>
import CookoutService from '../services/CookoutService';
import FoodCard from "./FoodCard.vue";
import Cart from './Cart.vue';

export default{
    data(){
      return{
        menu:[],
        cart: {
        userId: this.$store.state.user.userId,
        cookoutId: this.$route.params.id,
        menuItems: [],
        isCompleted: 0,
      },
      }
    },
    methods: {
        getMenuItems() {
          CookoutService.GetCookoutMenu(this.$route.params.id).then(response => {
            this.menu = response.data;
          })
        },
        setActiveCookout() {
          CookoutService.GetCookoutByCookoutID(this.$route.params.id).then(response => {
            this.$store.commit("SET_ACTIVE_COOKOUT", response.data);
          })
        },
        addItem(item) {
          let index = this.$store.state.cart.length;
          item.position = index;
          this.cart.menuItems.push(item)
          this.$store.commit("ADD_ITEM_TO_CART", item)
        },
        showSnackBug() {
          var x = document.getElementById("order-snack");
          x.className = "show";
          setTimeout(function(){x.className = x.className.replace("show","");},3000);
        },
    },
    created() {
        this.getMenuItems();
        this.setActiveCookout();
    },

    
  name: "menu-list",
  components: {
    FoodCard,
    Cart,
  },
};
</script>

<style>
body {
  background-image: linear-gradient(#3a3a3b,rgb(99, 98, 98));
}
h1 {
  color: #f2af29;
  font-size: 3rem;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  width: 100%;
  text-align: center;
}
.foodcard {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-evenly;
}

#menu-list-title{
  font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#menu-setup{
  width: 140px;
  height: 40px;
  background-color: #F2AF29;
  border: 2px solid #F2AF29;
  color:#3a3a3b;
  font-size: 1em;
  font-weight: bold;
  cursor:pointer;
  margin-bottom: 20%;
  transition: .3s ease;
}

#menu-setup:hover{
  border: 2px solid #f8f8ff;
  color:#f8f8ff;
  transition: .3s ease;
}

.menu-list{
  display: flex;
  align-content: center;
  justify-content: center;
  flex-wrap: wrap;
}

.menu-setup-link{
  width: 10%;
}
#order-snack {
  visibility: hidden;
  min-width: 250px;
  margin-left: -125px;
  background-color: #333;
  color: lightgreen;
  text-align: center;
  border-radius: 2px;
  padding: 16px;
  position: fixed;
  z-index: 10000;
  left: 50%;
  top:100px;
  font-size: 22px;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#order-snack.show {
  visibility: visible;
  -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
  animation: fadein 0.5s, fadeout 0.5s 2.5s;
}
#cart-navigation{
  width: 140px;
  height: 40px;
  background-color: #F2AF29;
  border: none;
  color:#3a3a3b;
  font-size: 1em;
  font-weight: bold;
  cursor:pointer;
}

.drop-step-enter, .drop-step-leave-to{
  opacity: 0;
  max-height: 0;
}

.drop-step-enter-to, .drop-step-leave{
  opacity: 1;
  max-height: 3000px;
}

.drop-step-enter-active, .drop-step-leave-active{
  transition: all .8 ease;
}

@media only screen and (max-width: 768px) {
  h1{
    font-size: 2.5em;
  }

}
</style>