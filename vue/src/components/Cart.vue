<template>
  <div class="cart-view">
    <h1>Your Cart</h1>
    <table id="cart-list-table">
      <tr>
        <th class="cart-table-head cart-item-id">Remove</th>
        <th class="cart-table-head cart-item-name">Name</th>
        <th class="cart-table-head cart-item-description">Description</th>
        <th class="cart-table-head cart-item-price">Price</th>
      </tr>
    <tr class="cart-items" v-for="menuItem in this.$store.state.cart" v-bind:key="menuItem.index">
      <td class="cart-table-data cart-item-id">
        <svg v-on:click="RemoveFromCart(menuItem)" id="remove-cart-item-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M261-120q-24 0-42-18t-18-42v-570h-41v-60h188v-30h264v30h188v60h-41v570q0 24-18 42t-42 18H261Zm106-146h60v-399h-60v399Zm166 0h60v-399h-60v399Z"/></svg>
      </td>
      <td class="cart-table-data cart-item-name">{{menuItem.name}}</td>
      <td class="cart-table-data cart-item-description">{{menuItem.description}}</td>
      <td class="cart-table-data cart-item-price">${{menuItem.price.toFixed(2)}}</td>
    </tr>
    <tr id="final-cart-row">
      <td class="cart-table-data cart-item-id"></td>
      <td class="cart-table-data cart-item-name"></td>
      <td class="cart-table-data cart-item-description"></td>
      <td class="cart-table-data cart-item-price">Total</td>
    </tr>
    <tr id="final-price-row">
      <td class="cart-table-data cart-item-id"></td>
      <td class="cart-table-data cart-item-name"></td>
      <td class="cart-table-data cart-item-description"></td>
      <td class="cart-table-data cart-item-price">${{this.subtotal.toFixed(2)}}</td>
    </tr>
    </table>
    <button class="submit-order-btn" v-on:click="this.CreateOrder">Send Order</button>
  </div>
</template>

<script>
import OrderService from '../services/OrderService';
export default {

  data() {
    return {
      newOrder: {
        isCompleted: false,
        email: this.$store.state.user.username === "user@gmail.com" ? this.$store.state.user.currentEmail : this.$store.state.user.username,
        cookoutId: Number(this.$route.params.id),
        menuItems: this.$store.state.cart
      },
      cart: [],
    };
  },
  methods: {
    CreateOrder(){
      OrderService.CreateOrder(this.$route.params.id, this.newOrder);
      this.newOrder = []
      this.$store.commit("CLEAR_CART");
      alert("Order Submitted!");
      this.$router.push(`/cookout/${this.$route.params.id}`)
    },
    RemoveFromCart(item){
      this.$store.commit("REMOVE_FROM_CART", item.position)
      let cart = this.$store.state.cart;
      cart.forEach((item)=>{
        item.position -= 1
      })
    }
  },
  computed: {
    subtotal(){
      let prices =[];
      this.$store.state.cart.forEach(element => {
        prices.push(element.price);
      });

      let sum = prices.reduce((previous,current)=> {
        return previous + current;
      },0);
      return sum;
    },
  }
  
};
</script>

<style>
.cart-view{
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items:center
}

#cart-list-table{
  width: 100%;
}

.cart-table-head{
  color: #f2af29;
  text-decoration: underline;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.2em;
}

.cart-table-data{
  text-align: center;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:#f8f8ff;
}

#final-cart-row{
  height: 60px;
  text-align: center;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:#f8f8ff;
}

#final-price-row{
  text-align: center;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:#f8f8ff;
}

.cart-items td{
  border-bottom: 2px solid #f8f8ff83;
}

.submit-order-btn{
  width: 180px;
  min-width: 90px;
  height: 35px;
  min-height: 25px;
  border: 2px solid #f2af29;
  background-color: #f2af29;
  color: #3a3a3b;
  font-size: 1em;
  font-weight: bold;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  transition: .3s ease;
}

.submit-order-btn:hover{
  border: 2px solid #f8f8ff;
  color:#f8f8ff;
  transition: .3s ease;
}

.cart-item-id{
  width: 10%;
}

.cart-item-name{
  width: 20%;
}

.cart-item-description{
  width: 60%;
}

.cart-item-price{
  font-weight: bold;
  width: 10%;
}

#remove-cart-item-icon{
  fill: #f8f8ff;
  width: 32px;
   transition: all .4s ease;
}

#remove-cart-item-icon:hover{
  fill: #d74c4c;
  transition: all .4s ease;
  animation: shake .4s;
}

@keyframes shake {
 0% { transform: translateX(0) }
 25% { transform: translateX(2px) }
 50% { transform: translateX(-2px) }
 75% { transform: translateX(2px) }
 100% { transform: translateX(0) }
}


</style>