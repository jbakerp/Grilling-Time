<template>
  <li class="order-card-container show" v-if="this.selectedOrder.isCompleted == false" @click="this.setActiveOrder">
      <div class="order-tagline">
        <h2 class="order-card-header">{{order.email}}</h2>
        <button id="finish-order-btn" @click="finishOrder" type="submit">Finish Order</button>
      </div>
      <section class="orders-details-section">
        <div class="order-item-container" v-for="item in items" v-bind:key="item.itemId">
          <div class="order-details-line">
              <h3 class="order-item-name">{{item.name}}</h3>
              <h3 class="order-item-description">{{item.description}}</h3>
              <h3 class="order-item-price">$ {{(item.price).toFixed(2)}}</h3>
          </div>
        </div>
      </section>
  </li>
</template>

<script>
import OrderService from "../services/OrderService.js"
export default {
    props:["order", "orderList"],
    data() {
        return {
            items:[],
            orderId: this.order.orderId,
            selectedOrder: {
                isCompleted: this.order.isCompleted

            }
        }
    },
    methods: {
        getItemsPerOrder(){
            OrderService.GetItemsByOrderId(this.orderId, this.$route.params.id).then(response =>{
                this.items = response.data;
            })
        },  
        getOrderByOrderId(){
            OrderService.GetOrderByOrderId(this.$route.params.id,this.orderId).then(response => {
                this.selectedOrder = response.data;
            })
        },
        setActiveOrder(){
            this.$store.commit("SET_ACTIVE_ORDER", this.selectedOrder)
        },
        finishOrder(){
            this.$emit('order-complete');
            this.setActiveOrder();
            this.$store.state.activeOrder.isCompleted = true;
            this.selectedOrder =  this.$store.state.activeOrder
            OrderService.UpdateOrder(this.selectedOrder, this.$route.params.id);

        },       
    },

    created(){
        this.getItemsPerOrder();
        this.getOrderByOrderId();
    },
}
</script>

<style>
.order-card-container{
    width: 90%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    background-color: lightslategrey;
    border: 2px solid #f8f8ff;
    margin-bottom: 1%;
    border-radius: 12px;
    transition: all .3s ease;
}

.order-tagline{
    display: flex;
    align-items: center;
    width: 100%;
    justify-content: space-between;
    margin-left: 2%;
}

#finish-order-btn{
    height: 40%;
    min-width: 90px;
    margin-right: 4%;
    justify-self: flex-end;
    border:none;
    color: #f8f8ff;
    background-color: #3a3a3b;
    margin-left: 5%;
    font-size: 1em;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    cursor: pointer;
    padding: 10px;
    border: 2px solid #3a3a3b;
    transition: .3s ease;
    text-align: center;
}

#finish-order-btn:hover{
    border: 2px solid lightgreen;
    color: lightgreen;
    transition: .3s ease;
}

.orders-details-section{
    background-color: rgba(0,0,0,.5);
    width: 90%;
    margin-bottom: 2%;
}

.order-details-line{
    display: flex;
    justify-content: space-around;
    align-items: center;
    border-bottom: 2px solid #f8f8ffbd;
}

.order-details-line h3{
    color:#f8f8ff;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    font-size: 1.2em;
    text-align: center;
    
}

.order-card-header{
    color:#f8f8ff;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    font-size: 2em;
    justify-self: flex-start;
}
.order-item-price{
    width: 15%;
}

.order-item-description{
    width: 65%;
}
.order-item-name{
    width: 20%;
}


@media only screen and (max-width: 768px) {
    .order-tagline{
        display: flex;
        flex-direction: row;
        margin-bottom: 3%;
        margin-top: 3%;
    }
    .orders-details-section{
        width:100%;
    }
    .order-card-container{
        width: 100%;
    }
    .order-card-header{
        font-size: .8em;
    }
    .orders-details-section{
        font-size: .7em;
    }

}

</style>