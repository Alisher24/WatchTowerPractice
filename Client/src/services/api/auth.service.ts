import axios from 'axios';

const API_URL = 'http://localhost:5003/';

const register = (email, name, password) => axios.post(`${API_URL}auth/Register`, {
  email: email,
  name: name,
  password: password,
}).then((response) => {
  console.log(response);
  // login(email, password);
  localStorage.setItem('userName', name);
  return response.data;
});

const login = (name, password) => axios
  .post(`${API_URL}auth/Login`, {
    name: name,
    password: password,
  })
  .then((response) => {
    console.log(response);
    if (response.data.token) {
      localStorage.setItem('token', JSON.stringify(response.data));
    }
    return response.data;
  })
  .catch((error) => {
    console.error("Login error: ", error);
    throw error;
  });


const getCameras = () => {
  return axios.get(`${API_URL}camera/get-cameras`, {
    headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
  });
};

const registerCamera = (ip: string, name: string, password: string) => {
  return axios.post(`${API_URL}camera/register-camera`, { ip, name, password }, {
    headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
  });
};

const startStream = (ip: string, name: string, password: string) => {
  return axios.get(`${API_URL}stream/start-stream`, {
    headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
    params: { ip, name, password }
  });
};


export default {
  register,
  login,
  getCameras,
  registerCamera,
  startStream,
};
