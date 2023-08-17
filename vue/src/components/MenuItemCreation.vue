<template>
<div>
  <div class="new-item-form">
      <form v-on:submit.prevent>
          <label>Name of the Item</label>
          <input type="text" placeholder="Name of Item" v-model="newItem.name">
          <label> Item Description</label>
          <input type="description" placeholder="Description of the Item" v-model="newItem.description">
          <div id="item-attribute-check-container">
            <div id="vegan-check">
                <label>Is it Vegan?  </label>
                <input type="checkbox" v-on:click="newItem.isVegan = !newItem.isVegan">
            </div>
            <div id="vegetarian-check">
                <label>Is it Vegetarian? </label>
                <input type="checkbox" v-on:click="newItem.isVegetarian = !newItem.isVegetarian">
            </div>
            <div id="gluten-free-check">
                <label>Is it Gluten Free? </label>
                <input type="checkbox" v-on:click="newItem.isGlutenFree = !newItem.isGlutenFree">
            </div>
          </div>
          <label>Image to Display</label>
          <input type="text" placeholder="URL of the Item" v-model="newItem.image">
          <label v-show="$store.state.activeCookout.hostId === $store.state.user.userId">Price of Item</label>
          <input type="text" placeholder="Price of the Item" v-model="newItem.price" v-show="$store.state.activeCookout.hostId === $store.state.user.userId">
          <button class="set-menu-buttons" type="submit" v-on:click="addItemToMenu">Submit new Item</button>
      </form>
  </div>
</div>
</template>

<script>

export default {
    data() {
        return{
            newItem: {
                cookoutId: Number(this.$route.params.id),
                description: '',
                image: '',
                isAvailable: true,
                isVegan: false,
                isVegetarian: false,
                isGlutenFree: false,
                name: '',
                price: '',
                isGuestBrought: false,
            }
        }
    },
    methods: {
     addItemToMenu() {
         if(this.$store.state.user.userId === this.$store.state.activeCookout.hostId){
             this.newItem.isGuestBrought = false;
         }
         else{
             this.newItem.isGuestBrought = true;
         }
         if(this.newItem.price == ''){
             this.newItem.price = 0
         }
         else{
            this.newItem.price = Number(this.newItem.price);
         }
         this.$emit('addItem', this.newItem);
         this.newItem = [];
     }
 }
}
</script>

<style>
.new-item-form{
  text-align: center;
  margin-top:5%;
}
.new-item-form form{
  display: flex;
  flex-direction: column;
  width: 95%;
  margin-right: 3%;
  margin-left: 3%;
  align-items: center;
  justify-content: center;
  margin-bottom: 5%;
}
.new-item-form form input {
    width: 100%;
    margin-bottom: 20px;
}
.new-item-form form #vegan-check {   
    justify-content: space-evenly;
}

.new-item-form label{
    align-self: flex-start;
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
    color:#F2AF29;
    margin-bottom: 1%;
}

#item-attribute-check-container{
    display: flex;
    align-items: center;
    justify-content: center;
}


</style>