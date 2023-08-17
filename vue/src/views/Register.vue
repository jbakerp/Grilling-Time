<template>
  <div id="register" class="text-center">
    <form id="register-form" @submit.prevent="register">
      <h1>Create Account</h1>
      <div role="alert" v-if="registrationErrors">
        {{ registrationErrorMsg }}
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input type="text" id="username" placeholder="Username" v-model="user.username" required autofocus />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input type="password" id="password" placeholder="Password" v-model="user.password" required />
      </div>
      <div class="form-input-group">
        <label for="confirmPassword">Confirm Password</label>
        <input type="password" id="confirmPassword" placeholder="Confirm Password" v-model="user.confirmPassword" required />
      </div>
      <button type="submit">Create Account</button>
      <p><router-link id="login-link" :to="{ name: 'login' }">Already have an account? Log in.</router-link></p>
    </form>
  </div>
</template>

<script>
import authService from '../services/AuthService';

export default {
  name: 'register',
  data() {
    return {
      user: {
        username: '',
        password: '',
        confirmPassword: '',
        role: 'user',
      },
      registrationErrors: false,
      registrationErrorMsg: 'There were problems registering this user.',
    };
  },
  methods: {
    register() {
      if (this.user.password != this.user.confirmPassword) {
        this.registrationErrors = true;
        this.registrationErrorMsg = 'Password & Confirm Password do not match.';
      } else {
        authService
          .register(this.user)
          .then((response) => {
            if (response.status == 201) {
              this.$router.push({
                path: '/login',
                query: { registration: 'success' },
              });
            }
          })
          .catch((error) => {
            const response = error.response;
            this.registrationErrors = true;
            if (response.status === 400) {
              this.registrationErrorMsg = 'Bad Request: Validation Errors';
            }
          });
      }
    },
    clearErrors() {
      this.registrationErrors = false;
      this.registrationErrorMsg = 'There were problems registering this user.';
    },
  },
};
</script>

<style scoped>
.form-input-group {
  margin-bottom: 1rem;
}
label {
  margin-right: 0.5rem;
}

#register{
  display: flex;
  flex-direction: column;
  align-items:center;
  justify-content: center;
  width:100%;
}
#register-form{
  display: flex;
  flex-direction: column;
  align-items:center;
  justify-content: center;
  width: 80%;
}

.form-input-group{
  width: 35%;
}

.form-input-group input:focus{
  border: none;
}

#register div{
  display: flex;
  flex-direction: column;
  margin-bottom: 3%;
}

#register div label{
  align-self: flex-start;
  margin-bottom: 3%;
  color:#F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  font-size: 1.5em;
}

#register button{
  height:40px;
  border: none;
  background-color: #F2AF29;
  color:#3a3a3b;
  font-size: 1em;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-weight: bold;
  cursor: pointer;
  min-width: 200px;
  margin-bottom: 2%;
}


#register div input{
  width: 100%;
  height: 25px;
  font-size: .8em;
  border:none;
}

#login-link{
  color: #e4e4e7;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
}
#login-link:hover{
  color: #F2AF29;
  font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
  font-size: 1.3em;
  transition: .4s ease-in;
}

</style>
