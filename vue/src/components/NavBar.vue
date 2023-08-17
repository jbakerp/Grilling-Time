<template>
  <div id="nav">
    <div id="icon-container">
      <svg
        key="1"
        class="menu-icon"
        xmlns="http://www.w3.org/2000/svg"
        height="48"
        viewBox="0 -960 960 960"
        width="48"
        @click="toggleDropdown"
        v-if="this.$store.state.dropdownOpen === false"
      >
        <path
          d="M120-240v-60h720v60H120Zm0-210v-60h720v60H120Zm0-210v-60h720v60H120Z"
        />
      </svg>
      <svg
        key="2"
        class="menu-icon"
        xmlns="http://www.w3.org/2000/svg"
        height="48"
        viewBox="0 -960 960 960"
        width="48"
        @click="toggleDropdown"
        v-else-if="this.$store.state.dropdownOpen === true"
      >
        <path
          d="m249-207-42-42 231-231-231-231 42-42 231 231 231-231 42 42-231 231 231 231-42 42-231-231-231 231Z"
        />
      </svg>
      <drop-down-menu />
    </div>
    <div class="title-text">GRILLING TIME!</div>
    <div id="nav-btns">
      <router-link id="home-btn" v-bind:to="{ name: 'Cookouts' }">Home</router-link
      >&nbsp;|&nbsp;
      <router-link
        id="logout-btn"
        v-bind:to="{ name: 'logout' }"
        v-if="$store.state.token != ''"
        >Logout</router-link>
    </div>
  </div>
</template>

<script>
import DropDownMenu from "./DropDownMenu.vue";
export default {
  components: { DropDownMenu },
  methods: {
    toggleDropdown() {
      this.$store.commit("TOGGLE_DROPDOWN");
    },
  },
};
</script>

<style>
#nav {
  display: flex;
  position: sticky;
  align-items: center;
  justify-content: flex-end;
  top: 0;
  width: 100%;
  background-color: #3a3a3b;
  border-bottom: 2px solid #f8f8ff;
  z-index: 9999;
}

#grilly-container{
  width: 33%;
}

.title-text {
  font-size: 4em;
  color: #f2af29;
  width: 33%;
  font-family: Impact, Haettenschweiler, "Arial Narrow Bold", sans-serif;
  justify-self: center;
  text-align: center;
  position: sticky;
  margin: 0;
}

#nav-btns {
  font-size: 1.5em;
  text-decoration: none;
  height: 100%;
  display: flex;
  color: darkgrey;
  justify-content: flex-end;
  width: 33%;
  margin-right: 1em;
}

#icon-container {
  width: 33%;
  margin-left: 2%;
  display: none;
}

.menu-icon {
  fill: #f8f8ff;
  transition: 0.4s ease;
}

.menu-icon:hover {
  fill: #f2af29;
  transition: 0.4s ease;
}

#nav-btns a {
  text-decoration: none;
  color: #f8f8ff;
  font-family: Impact, Haettenschweiler, "Arial Narrow Bold", sans-serif;
  transition: 0.4s;
}

#nav-btns a:hover {
  text-decoration: none;
  color: #f2af29;
  text-decoration: underline;
  font-family: Impact, Haettenschweiler, "Arial Narrow Bold", sans-serif;
  transition: 0.4s;
}

#shopping-cart-icon{
  fill:#f8f8ff;
  height: 30px;
}

@media only screen and (max-width: 768px) {
  #nav {

  position: sticky;
  top: 0;
  width: 100%;
  background-color: #3a3a3b;
  border-bottom: 2px solid #f8f8ff;
  z-index: 9999;
  justify-content: space-around;
  font-size: .7em;
  }
#nav-btns{
  display: none;
}

#icon-container{
  width: 10%;
  display: inline-block;
}

.title-text{
  justify-self: center;
  width: 90%;
}

}


</style>