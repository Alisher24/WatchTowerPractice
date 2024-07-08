import axios from 'axios';

const API_URL = 'change api url in here (http://localhost:5000/api/)';

const register = (email, name, password) => axios.post(`${API_URL}auth/signup`, {
  "email": email,
  "name":name,
  "password": password,
}).then((response) => {
  console.log(response);
  // login(email, password);
  localStorage.setItem('userName', name);
  return response.data;
});

const login = (name, password) => axios
  .post(`${API_URL}auth/Login`, {
    "name": name,
    "password": password,
  })
  .then((response) => {
    console.log(response);
    if (response.data.token) {
      localStorage.setItem('tokens', JSON.stringify(response.data));
    }
    return response.data;
  });


export default {
  register,
  login,
};
