<template>
  <div id="app">
    <nav-bar/>
    <router-view />
  </div>
</template>

<script>
import NavBar from './components/NavBar.vue'

export default {
  components: {
    NavBar
  }
}

</script>




<style>
body{
  margin:0;
  box-sizing: border-box;
}
*{
  box-sizing: border-box;
}


body::-webkit-scrollbar {
    display: none;
}

/* Hide scrollbar for IE, Edge and Firefox */
body {
  -ms-overflow-style: none;  /* IE and Edge */
  scrollbar-width: none;  /* Firefox */
}
#app{
  min-height: 110vh;
  max-width: 100vw;
}

@media only screen and (max-width: 768px) {
  h1{
    font-size: 2.5em;
  }
}

</style>