<template>
  <div id="guest-login-container">
      <h1>Guest Login</h1>
      <div class="guest-login-form-block">
          <form class="guest-login-form" @submit.prevent="login">
            <label id="username-guest-label" for="username">Email</label>
            <input type="text" placeholder="Email" id="username" v-model="user.email" required autofocus />
            <button type="submit">Sign in</button>
            <p><router-link id="login-link" :to="{ name: 'login' }">Already have an account? Log in.</router-link></p>
          </form>
      </div>
  </div>
</template>

<script>
import authService from "../services/AuthService";
export default {
    data() {
        return {
        user: {
            username: "user@gmail.com",
            password: "password",
            role: 'user',
            email: ""
      },
    };
  },
  methods: {
    login() {
      authService.login(this.user).then(response => {
          if (response.status == 200) {
            this.$store.commit("SET_AUTH_TOKEN", response.data.token);
            this.$store.commit("SET_CURRENT_EMAIL",this.user.email)
            this.$store.commit("SET_USER", response.data.user); 
            this.$router.push("/");
          }
        }).catch(error => {
          const response = error.response;
          if (response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    }
  }
};

</script>

<style>
#guest-login-container{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 100%;
}


.guest-login-form{
  display: flex;
  flex-direction: column;
  width: 100%;
  align-items: center;
  height: 300px;
}

.guest-login-form label{
  align-self: flex-start;
  color: #F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  margin-bottom: 3%;
  font-size: 1.3em;
}

.guest-login-form button{
  width: 180px;
  height: 35px;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  background-color: #F2AF29;
  border: 2px solid #F2AF29;
  color: #3a3a3b;
  margin-top: 10%;
  transition: .3s ease;
  cursor: pointer;
}

.guest-login-form button:hover{
  border:2px solid #f8f8ff;
  color: #f8f8ff;
  transition: .3s ease;
}

#login-link{
  color: #e4e4e7;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .3s ease-in;
}

#login-link:hover{
  color: #F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .3s ease-in;
}


</style>