import Vue from 'vue'
import Router from 'vue-router'
import Login from '../views/Login.vue'
import Logout from '../views/Logout.vue'
import Register from '../views/Register.vue'
import store from '../store/index'
import InviteForm from '../views/SendInvites.vue'
import Reserve from '../views/RSVP.vue'
import Menu from '../views/Menu.vue'
import CreateCookout from '../views/CreateCookout.vue'
import Cookouts from '../views/Cookouts.vue'
import CookoutDetails from '../views/CookoutDetails.vue'
import ShortOrderList from '../views/ShortOrderList.vue'
import Cart from '../views/Cart.vue'
import SetMenu from '../views/SetMenu.vue'
import GuestLogin from '../views/GuestLogin.vue'

Vue.use(Router)

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/logout",
      name: "logout",
      component: Logout,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/register",
      name: "register",
      component: Register,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/cookout/:id/invites",
      name: 'Invites',
      component: InviteForm,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/cookout/:id/rsvp/:inviteId",
      name: 'rsvp',
      component: Reserve,
    },
    {
      path: "/cookout/:id/menu",
      name: "Menu",
      component: Menu
    },
    {
      path: "/create-cookout",
      name: 'Create-Cookout',
      component: CreateCookout,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/cookout/:id",
      name: 'CookoutDetails',
      component: CookoutDetails,
    },
    {
      path: "/",
      name: 'Cookouts',
      component: Cookouts,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/cookout/:id/orders",
      name: 'ShortOrderList',
      component: ShortOrderList
    },
    {
      path: "/cookout/:id/cart",
      name: 'Cart',
      component: Cart
    },
    {
      path: "/cookout/:id/setMenu",
      name: 'setMenu',
      component: SetMenu,
    },
    {
      path:"/login/guest",
      name: 'GuestLogin',
      component: GuestLogin,
    },



  ]
})

router.beforeEach((to, from, next) => {
  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && store.state.token === '') {
    next("/login");
  } else {
    // Else let them go to their next destination
    next();
  }
});

export default router;
