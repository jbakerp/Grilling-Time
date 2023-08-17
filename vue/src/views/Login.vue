<template>
  <div id="login">
    <form @submit.prevent="login">
      <h1 >Please Sign In</h1>
      <div role="alert" v-if="invalidCredentials">
        Invalid username and password!
      </div>
      <div role="alert" v-if="this.$route.query.registration">
        Thank you for registering, please sign in.
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input type="text" placeholder="Username" id="username" v-model="user.username" required autofocus />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input type="password" placeholder="Password" id="password" v-model="user.password" required />
      </div>
      <button type="submit">Sign in</button>
      <p>
      <router-link id="register-btn" :to="{ name: 'register' }">Need an account? Sign up.</router-link></p>
        <p><router-link id="guest-login" :to="{name: 'GuestLogin'}">Login as a Guest</router-link></p>
    </form>
  </div>
</template>

<script>
import authService from "../services/AuthService";

export default {
  name: "login",
  components: {},
  data() {
    return {
      user: {
        username: "",
        password: ""
      },
      invalidCredentials: false
    };
  },
  methods: {
    login() {
      authService
        .login(this.user)
        .then(response => {
          if (response.status == 200) {
            this.$store.commit("SET_AUTH_TOKEN", response.data.token);
            this.$store.commit("SET_USER", response.data.user);
            this.$router.push("/");
          }
        })
        .catch(error => {
          const response = error.response;

          if (response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    }
  }
};
</script>

<style scoped>
.form-input-group {
  margin-bottom: 1rem;
}
label {
  margin-right: 0.5rem;
}

#login{
  width:100%;
  display:flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

#login form{
  width: 60%;
  display:flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

#login div{
  display:flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: center;
  width: 40%;
  margin-bottom: 3%;
}


#login div label{
  align-self: flex-start;
  margin-bottom: 3%;
  color:#F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  font-size: 1.5em;
}

#login div input{
  width: 100%;
  height: 25px;
  font-size: .8em;
}

#login button{
  width: 15%;
  height:40px;
  border: none;
  background-color: #F2AF29;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 3%;
  min-width: 200px;
}

#register-btn{
  color: #f8f8ff;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .4s ease-in;
}

#register-btn:hover{
  color: #F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .4s ease-in;
}

#guest-login{
  color: #f8f8ff;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .4s ease-in;
}

#guest-login:hover{
  color: #F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .4s ease-in;
}

</style>