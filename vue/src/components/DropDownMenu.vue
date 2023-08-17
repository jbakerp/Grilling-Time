<template>
<transition name="fade" appear>
  <div class="menu-item show" v-show="$store.state.dropdownOpen" >
      <router-link class="dropdown-links" v-bind:to="{name: 'Cookouts'}" >Cookouts</router-link>
      <router-link class="dropdown-links" v-bind:to="{name: 'Create-Cookout'}" >Create Cookout</router-link>
      <router-link class="dropdown-links" v-bind:to="{name: 'logout'}"> Logout</router-link>
  </div>
  </transition>
</template>

<script>
export default {
    data() {
        return{
            isOpen: false,
        }
    },
    methods:{
        toggleDropdown(){
            this.$store.commit("TOGGLE_DROPDOWN");
        }
    }
}
</script>


<style>
html{
    box-sizing: border-box;
}
.menu-item{
    display: flex;
    flex-direction: column;
    position: absolute;
    background-color: #3a3a3b;
    top: calc(100% + 2px);
    transform: translateX(-10%);
    border-left: 2px solid #f8f8ff;
    border-right: 2px solid #f8f8ff;
    margin-left: calc(1% + 4px);
}

.menu-item .dropdown-links{
    text-decoration: none;
    color: #f8f8ff;
    font-size: 2em;
    font-family: Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
    border-bottom: 2px solid #f8f8ff;
    text-align: center;
    padding: 10px 10px 10px 10px;
    border-bottom: 4px solid #f8f8ff;
    transition: all .3s ease;
}

.menu-item .dropdown-links:hover{
    color: #F2AF29;
    transition: all .3s ease;
    border-bottom: 4px solid #F2AF29;
}

.fade-enter-active, .fade-leave-active{
    transition: all .3s ease-out;
}

.fade-enter, .fade-leave-to{
    opacity: 0;
}

@media only screen and (max-width: 768px) {
    .dropdown-links{
        width: 100%;
    }
    .menu-item{
        width: 100%;
        left: 8%;
        background-color: #3a3a3be0;
    }    



}




</style>