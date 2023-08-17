<template>
  <div
    class="card"
    v-bind:style="{ 'background-image': `url(${menuItem.image})` }"
  >
    <div class="item-attribute-container">
      <div class="item-bubble item-price-bubbe" v-if="menuItem.price != 0">
        ${{ menuItem.price.toFixed(2) }}
      </div>
      <div
        class="item-bubble item-vegan-bubble"
        v-if="menuItem.isVegan == true"
      >
        VE
      </div>
        <div class="item-bubble item-vegetarian-bubble" v-if="menuItem.isVegetarian">VGT</div>
        <div class="item-bubble item-gluten-free-bubble" v-if="menuItem.isGlutenFree">GF</div>
    </div>
    <div class="order-snack" v-bind:key="menuItem.itemID">Added to order!</div>
    <div class="button-container">
      <button class="add-to-order" @click="addItem(menuItem)">
        Add to Order
      </button>
    </div>
    <div class="card-info">
      <h2 class="name">{{ menuItem.name }}</h2>
      <p class="description">
        {{ menuItem.description }}
      </p>
    </div>
  </div>
</template>

<script>
//import services from "../services/MenuService.js";
import OrderService from "../services/OrderService.js";

export default {
  data() {
    return {
      item: {
        itemId: 0,
        name: "",
        description: "",
        price: 0.0,
        image: "",
        is_vegan: false,
        is_available: true,
      },
    };
  },
  methods: {
    addItem() {
      this.$emit('addItemToOrder', this.menuItem)
      },
    getOrders() {
      OrderService.GetOrderByOrderId(this.orderId).then((response) => {
        this.order = response.data;
      });
    },
    createOrder() {
      OrderService.CreateOrder(this.newOrder)
        .then((response) => {
          if (response.status === 201) {
            this.getOrder(this.userId);
            this.newOrder = {
              name: "",
              itemId: "",
              price: 0,
              userId: "",
            };
          }
        })
        .catch((error) => {
          if (error.response) {
            this.errorMsg =
              `Error ${error.response.status} Order was not posted.` +
              error.response.statusText +
              "'.";
          } else if (error.request) {
            this.errorMsg = "Error adding order. Server could not be reached.";
          } else {
            this.errorMsg = "Error adding order. Request could not be sent.";
          }
        });
    },
    addedItem(){
      
    },    
  },
  name: "food-card",
  props: ["menuItem"],
}
</script>

<style>
.template {
  background-color: lightslategray;
}

.card {
  border: 2px solid #f2af29;
  border-radius: 12px;
  width: 550px;
  height: 300px;
  margin-bottom: 20px;
  background-size: 550px 550px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  transition: 1.75s ease-in-out;
  background-color: #f2af29;
  margin-bottom: 1%;
}
.card:hover {
  box-shadow: 0px 0px 3px 3px orange;
  transform: scale(1.02);
  transition: 0.3s ease;
}

.card .name {
  font-size: 2.5rem;
  text-align: center;
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial, sans-serif;
  color: #f8f8ff;
  opacity: 1;
  margin: 0;
  margin-top: 1%;
  text-shadow: 1px 0px black;
  transition: 0.8s ease-in-out;
}

.card .description {
  font-size: 1.2em;
  text-align: center;
  color: #f8f8ff;
  opacity: 0;
  transition: 0.8s ease-in-out;
}
.card .button-container {
  display: absolute;
  text-align: center;
  top: 50;
  left: 50;
  justify-content: center;
  margin-top: 5%;
}

.add-to-order {
  border: none;
  border-radius: 8px;
  font-weight: bolder;
  width: 40%;
  height: 30px;
  font-size: 1em;
  cursor: pointer;
}

.add-to-order:hover {
  background-color: palegreen;
  border: 1px solid black;
  transition: 0.4s ease-in-out;
}

.card-info {
  background-image: linear-gradient(
    to top,
    rgba(11, 11, 11, 0.95),
    50%,
    rgba(11, 11, 11, 0.2)
  );
  justify-self: flex-end;
  max-height: 25%;
  border-radius: 12px;
  transition: 1.5s ease-in-out;
  overflow-y: hidden;
}

.item-attribute-container {
  width: 100%;
  display: flex;
  flex-direction: row;
  justify-content: center;
  margin-top: 10px;
}

.item-bubble {
  background-color: rgba(11, 11, 11, 0.9);
  border-radius: 24px;
  min-width: 70px;
  width: 100px;
  height: 40px;
  color: #f8f8ff;
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial, sans-serif;
  margin-left: 2%;
  margin-right: 2%;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 1s ease-in-out;
}

.card:hover .item-bubble {
  border: 1px solid #f8f8ff;
  transition: 1s ease-in-out;
}

.card:hover p {
  opacity: 1;
  transition: 1s ease-in-out;
}

.card:hover .card-info {
  max-height: 50%;
  transition: 0.35s ease-in-out;
}

.card:hover .name {
  text-shadow: 0px -1px 3px #fff,
    /* Innermost layer - intense heat (white) */ 0px -2px 6px #ff3,
    /* Second layer - core of flame (yellow) */ 0px -6px 12px #f90,
    /* Middle layer - body of flame (orange) */ 0px -10px 20px #c33; /* Outermost layer - edges of flame (red) */
  transition: 0.5s ease-in-out;
}
.order-snack {
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

.order-snack-show {
  visibility: visible;
  -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
  animation: fadein 0.5s, fadeout 0.5s 2.5s;
}


@media only screen and (max-width: 768px) {

  .card{
    width: 350px;
    height: 275px;
  }
}
</style>