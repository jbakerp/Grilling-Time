<template>
  <div id="set-menu-container">
    <router-link id="setmenu-back-button" class="details-btn-link" v-bind:to="{name: 'CookoutDetails', params:{id: this.$route.params.id}}">
    <button class="back-to-details-btn">
      <svg class="details-back-arrow-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m274-450 248 248-42 42-320-320 320-320 42 42-248 248h526v60H274Z"/></svg>
      Back to Details
    </button>
    </router-link>
    <h1>Set Your Cookout Menu Options</h1>
    <h2>Please Select All That Apply</h2>
    <div id="set-menu-card">
      <form id="menu-form" v-on:submit.prevent>
        <ul class="menu-checklist">
          <li class="item-selector"  v-for="menuItem in $store.state.defaultMenuItems" v-bind:menuItem="menuItem" v-bind:key="menuItem.itemID">
            <span class="menu-items" v-on:click="addSelectedItems(menuItem), isChecked = true">
              <div class="checklist-items">
                  <div class="check-circle-icon-container">
                    <transition name="appear">
                      <svg v-show="menuItem.currentlySelected == true" class="check-circle-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m421-298 283-283-46-45-237 237-120-120-45 45 165 166Zm59 218q-82 0-155-31.5t-127.5-86Q143-252 111.5-325T80-480q0-83 31.5-156t86-127Q252-817 325-848.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 82-31.5 155T763-197.5q-54 54.5-127 86T480-80Zm0-60q142 0 241-99.5T820-480q0-142-99-241t-241-99q-141 0-240.5 99T140-480q0 141 99.5 240.5T480-140Zm0-340Z"/></svg>
                    </transition>
                  </div>
                <div class="default-item-container">
                  <div class="item-name">{{menuItem.name}}</div>
                  <div>{{menuItem.description}}</div>
                </div>
              </div>
            </span>
          </li>
          <button class="set-menu-buttons" v-on:click="isVisible = !isVisible" v-show="isVisible == false">Add Additional Item</button>
          <button class="set-menu-buttons" v-on:click="isVisible = !isVisible" v-show="isVisible == true">Close Form</button>
        </ul>
        <transition name="slide">
          <menu-item-creation-vue @addItem="addSelectedItems" v-show="isVisible == true" />
        </transition>
        <hr class="division-line">
        <h2 class="selection-heading" v-show="selectedItems.length > 0">Selected Items -- Click to Remove Choice</h2>
        <transition name="slide">
        <ul class="selected-checklist" v-show="selectedItems.length > 0">
          <li class="item-selected" v-for="selectedItem in selectedItems" v-bind:selectedItem="selectedItem" v-bind:key="selectedItem.itemID">
            <span class="selected-items" v-on:click="removeSelectedItem(selectedItem), isChecked = false">
              <div class="cancel-circle-container">
                <svg class="cancel-circle-icon" xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m330-288 150-150 150 150 42-42-150-150 150-150-42-42-150 150-150-150-42 42 150 150-150 150 42 42ZM480-80q-82 0-155-31.5t-127.5-86Q143-252 111.5-325T80-480q0-83 31.5-156t86-127Q252-817 325-848.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 82-31.5 155T763-197.5q-54 54.5-127 86T480-80Zm0-60q142 0 241-99.5T820-480q0-142-99-241t-241-99q-141 0-240.5 99T140-480q0 141 99.5 240.5T480-140Zm0-340Z"/></svg>
              </div>
              <div class="default-item-container">
                <div class="item-name">{{selectedItem.name}}</div>
                <div>{{selectedItem.description}}</div>
              </div>
            </span>
            <li><button class="set-menu-buttons" v-show="selectedItems.length > 0" type="submit" v-on:click="createNewCookoutMenu()">Submit Menu</button>
          </li>
        </ul>
        </transition>
      </form>
    </div>
  </div>
</template>

<script>
import MenuService from '../services/MenuService';
import MenuItemCreationVue from './MenuItemCreation.vue';
export default {
  components: {MenuItemCreationVue}, 
  data() {
    return {
      menuItems: [
        {
          name: "",
          isAvailable: true,
        },
      ],
      selectedItems: [],
      isVisible: false,
    };
  },
  methods: {
    handleSubmit() {},
    addSelectedItems(item) {
      if(!this.selectedItems.includes(item)){
        item.cookoutId = Number(this.$route.params.id);
        this.selectedItems.push(item);
        let x = this.$store.state.defaultMenuItems.indexOf(item)
        this.$store.commit("SET_ITEM_CURRENTLY_SELECTED",x)
      }
    },
    removeSelectedItem(item){
      let index = this.selectedItems.indexOf(item);
      this.selectedItems.splice(index, 1);
      let x = this.$store.state.defaultMenuItems.indexOf(item)
      this.$store.commit("SET_ITEM_CURRENTLY_SELECTED",x)
    },
    createNewCookoutMenu() {
      this.selectedItems.forEach((item) => {
        MenuService.CreateMenuItem(item);
      });
      this.selectedItems = [];
      this.$router.push(`/cookout/${this.$route.params.id}`);
    }
  },
  created() {
  },
};
</script>

<style>
#set-menu-container {
  display: flex;
  flex-direction: column;
  width: 95%;
  margin-right: 3%;
  margin-left: 3%;
  align-items: center;
  justify-content: center;
  margin-bottom: 5%;
}
#set-menu-container h1 {
  font-style: oblique;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}
#set-menu-container h2 {
  color: #f2af29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  text-decoration-line: underline;
}
.menu-checklist{
  width: 100%;
  height:100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  padding: 0;
  margin-top: 3%;

}
.item-selector{
  border: 2px solid #f8f8ff;
  width: 95%;
  list-style: none;
  margin-bottom: 5px;
  background: lightslategrey;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.2em;
  justify-items: center;
  cursor: pointer;
}
.checklist-items{
    color: whitesmoke;
    display: flex;
    justify-content: flex-start;
    align-items: center;
}

.selector-header{
  text-align: center;
  color:whitesmoke;
}
.menu-items{
  text-align: center;
}
.item-selected{
  border: 2px solid #f8f8ff;
  width: 95%;
  list-style: none;
  margin-bottom: 5px;
  background: lightslategrey;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color: whitesmoke;
  font-size: 1.2em;
  justify-items: center;
  cursor: pointer;
  transition: .6s ease-out;
}
.division-line{
  width: 100%;
}
.selected-checklist{
  width: 100%;
  height:100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  padding: 0;
  margin-top: 3%;
  color: black;
  list-style: none;
}
.selected-items{
  display: flex;
  text-align: center;
  font-size: 1em;
}

.submit-button{
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

.set-menu-buttons{
  width: 180px;
  background-color: #F2AF29;
  border:2px solid #F2AF29 ;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  color:#3a3a3b;
  cursor: pointer;
  height:40px;
  font-weight: bold;
  transition: .3s ease;
  margin-top: 4%;
}

.set-menu-buttons:hover{
  color:#f8f8ff;
  border: 2px solid #f8f8ff;
  transition: .3s ease;
}

.check-circle-icon-container{
  width: 10%;
}

.cancel-circle-container{
  width: 10%;
  transition: .6s ease-out;
}

.cancel-circle-icon{
  fill: #f8f8ff;
  text-align:center;
}

.default-item-conatiner{
  width: 90%;
}

.default-item-container{
  width: 100%;
}

.item-name{
  text-decoration: underline;
  font-weight: bold;
}

.check-circle-icon{
  fill: lightgreen;
}

.appear-enter, .appear-leave-to{
  opacity: 0;
}

.appear-enter-to, .appear-leave{
  opacity: 1;
}
.appear-enter-active,.appear-leave-active{
  transition: all .9s;
}

#menu-form{
  width: 100%;
}

#set-menu-card{
  width: 100%;
}

#setmenu-back-button{
  margin-top:2%;
  align-self: flex-start;
}

.item-selected:hover .cancel-circle-icon{
  fill: rgba(235, 46, 46, 0.938);
  transition: .6s ease-out;
}

.item-selected:hover{
  border:2px solid rgba(235, 46, 46, 0.938);
  transition: .6s ease-out;
}

.slide-enter, .slide-leave-to{
  max-height: 0;
  opacity: 0;
  transform: translateY(-10%);
}

.slide-enter-to, .slide-leave{
  max-height: 5000px;
  opacity: 1;
  transform: translateY(0%);
}

.slide-enter-active, .slide-leave-active{
  transition: all .4s ease-in-out;
}

@media only screen and (max-width: 768px) {

  .item-selected{
    font-size: .9em;
  }
  .item-selector{
    font-size: .9em;
  }
  .check-circle-icon{
    width: 32px;
    height: 32px;
  }

  .cancel-circle-icon{
    width: 32px;
    height: 32px;
  }

}

</style>