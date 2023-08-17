import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

/*
 * The authorization header is set for axios when you login but what happens when you come back or
 * the page is refreshed. When that happens you need to check for the token in local storage and if it
 * exists you should set the header so that it will be attached to each request
 */

const currentToken = localStorage.getItem('token')
const currentUser = JSON.parse(localStorage.getItem('user'));


if (currentToken != null) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${currentToken}`;
}

export default new Vuex.Store({
  state: {
    token: currentToken || '',
    user: currentUser || {},
    currentEmail:'',
    menuItems: [],
    activeItem: {
      itemID: 0,
      name: '',
      description: '',
      image: '',
      isVegan: false,
      isVegetarian: false,
      isGlutenFree: false,
      isAvailable: true,
      isGuestBrought: false,
    },
    dropdownOpen: false,
    users: [],
    invites: [],
    invitesToShow: [],
    stateAbbrevs: ['AL', 'AK', 'AZ', 'AR', 'CA', 'CO',
      'CT', 'DE', 'DC', 'FL', 'GA', 'HI',
      'ID', 'IL', 'IN', 'IA', 'KS', 'KY',
      'LA', 'ME', 'MD', 'MA', 'MI', 'MN',
      'MS', 'MO', 'MT', 'NE', 'NV', 'NH',
      'NJ', 'NM', 'NY', 'NC', 'ND', 'OH',
      'OK', 'OR', 'PA', 'PR', 'RI', 'SC',
      'SD', 'TN', 'TX', 'UT', 'VT', 'VA',
      'VI', 'WA', 'WV', 'WI', 'WY',],

    activeCookout: {
      cookoutID: 0,
      hostId: 0,
      cookoutName: '',
      chefId: 0,
      dateOfEvent: '',
      location: {
        streetAddress: '',
        city: '',
        stateAbbreviation: '',
        zipcode: '',
      },
      description: '',
    },
    activeInvite: {
      inviteId: 0,
      inviteEmail: "",
      personName: "",
      isSent: true,
      cookoutId: 0,
      responseStatus: 0,
    },
    activeOrder: {
      orderId: 0,
      cookoutId: 0,
      isCompleted: false,
      userId: 0,
    },
    cart: [],
    currentCartIndex: 0,
    defaultMenuItems: [
      {
        "itemID": 1,
        "name": "Steak",
        "description": "A 8oz ribeye cooked medium-rare",
        "price": 10.99,
        "image": "https://greenvillejournal.com/wp-content/uploads/2020/07/Grilling-stock-1.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 2,
        "name": "Grilled Vegetables",
        "description": "An assortment of grilled peppers, onions, mushrooms, tomatoes, zucchinni, and squash",
        "price": 2.49,
        "image": "https://cf-images.us-east-1.prod.boltdns.net/v1/static/1033249144001/42161348-8d99-4cd6-9901-cb3cdf2df1a5/c8140d87-7c93-4e8e-acec-1fedb4c3eebf/1280x720/match/image.jpg",
        "isVegan": true,
        "isVegetarian": true,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 3,
        "name": "Hot Dog",
        "description": "A traditional ballpark frank grilled to perfection",
        "price": 1.50,
        "image": "https://i.ytimg.com/vi/3bXN9YCJGz0/maxresdefault.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": false,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 4,
        "name": "Burger",
        "description": "A juicy 1/4 lb patty on a fresh sesame seed bun",
        "price": 4.99,
        "image": "https://thekitchencommunity.org/wp-content/uploads/2020/12/Burger-Grill-Time-Chart-How-To-Grill-Burgers-1200x900.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": false,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 5,
        "name": "Corn on the Cob",
        "description": "A buttered, salted and beautifully charred corn on the cob",
        "price": 3.99,
        "image": "https://hips.hearstapps.com/hmg-prod/images/shot-2-0129-1522854796.png?crop=1xw:1xh;center,top&resize=1200:*",
        "isVegan": true,
        "isVegetarian":true,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 6,
        "name": "Ribs",
        "description": "A full fall-off-the-bone rack of ribs lathered in BBQ sauce ",
        "price": 11.99,
        "image": "https://wallpapercave.com/wp/wp2005466.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 7,
        "name": "Smoked Brisket",
        "description": "A mouthwatering combination of savory, smoky, and slightly sweet flavors",
        "price": 13.99,
        "image": "https://houseofnasheats.com/wp-content/uploads/2022/09/Texas-Smoked-Beef-Brisket-Recipe-Squaree-1.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 8,
        "name": "Smoked Wings",
        "description": "Rich, smokey and tender",
        "price": 6.99,
        "image": "https://disheswithdad.com/wp-content/uploads/2020/09/smoked-chicken-wings-1.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 9,
        "name": "Grilled Shrimp",
        "description": "Incredibly juicy and flavorful grilled shrimp skewers with veggies",
        "price": 5.99,
        "image": "https://littlesunnykitchen.com/wp-content/uploads/2021/06/Grilled-Shrimp-Kabobs-2.jpg",
        "isVegan": false,
        "isVegetarian": false,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      },
      {
        "itemID": 10,
        "name": "Grilled Watermelon",
        "description": "Sweet, salty, and smokey flavor that only leaves you wanting more",
        "price": 4.99,
        "image": "https://aarp-content.brightspotcdn.com/dims4/default/2c9ea39/2147483647/strip/false/crop/1280x704+0+0/resize/1280x704!/quality/90/?url=http%3A%2F%2Faarp-brightspot.s3.amazonaws.com%2Fcontent%2F9c%2F34%2Fa6ec36b743db9e24f6fbbede9cb8%2Fgrill-seasons-1280.jpg",
        "isVegan": true,
        "isVegetarian": true,
        "isGlutenFree": true,
        "isAvailable": true,
        "cookoutId": 0,
        "isGuestBrought": false,
        "currentlySelected": false,
      }
    ],

  },
  mutations: {
    SET_AUTH_TOKEN(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
    },
    SET_USER(state, user) {
      state.user = user;
      user.currentEmail = state.currentEmail
      localStorage.setItem('user', JSON.stringify(user))
    },
    LOGOUT(state) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      localStorage.removeItem('currentEmail');
      state.currentEmail= '';
      state.token = '';
      state.user = {};
      axios.defaults.headers.common = {};
    },
    SET_INVITES(state, data) {
      state.invites = data;
    },
    SET_INVITES_TO_SHOW(state, data) {
      state.invitesToShow = data;
    },
    SET_ACTIVE_INVITE(state, data) {
      state.activeInvite = data;
    },
    SET_MENU(state, data) {
      state.menuItems = data;
    },
    SET_ACTIVE_ITEM(state, data) {
      state.activeItem = data;
    },
    TOGGLE_DROPDOWN(state) {
      state.dropdownOpen = !state.dropdownOpen;
    },
    SET_USERS(state, data) {
      state.users = data;
    },
    ADD_INVITE(state, data) {
      state.invites.push(data);
    },
    SET_ACTIVE_COOKOUT(state, data) {
      state.activeCookout = data;
    },
    ADD_INVITE_TO_SEND(state, data) {
      state.selectedInvites.push(data);
    },
    REMOVE_INVITE(state, data) {
      let index = state.invites.indexOf(data);
      state.invites.splice(index, 1);
    },
    SET_ACTIVE_ORDER(state, data) {
      state.activeOrder = data;
    },
    SET_CURRENT_EMAIL(state,data){
      state.currentEmail = data;
      localStorage.setItem('currentEmail',data)
    },
    ADD_ITEM_TO_CART(state,data){
      state.cart.push(data);
    },
    CLEAR_CART(state){
      state.cart = [];
    },
    SET_CURRENT_CART_INDEX(state,data){
      state.currentCartIndex = data;
    },
    REMOVE_FROM_CART(state,index){
      state.cart.splice(index,1)
    },
    SET_ITEM_CURRENTLY_SELECTED(state,index){
      state.defaultMenuItems[index].currentlySelected = !state.defaultMenuItems[index].currentlySelected;
    },
    SET_ALL_ITEMS_NOT_SELECTED(state){
      state.defaultMenuItems.forEach((item)=>{
        item.currentlySelected = false;
      })
    }
  },
})