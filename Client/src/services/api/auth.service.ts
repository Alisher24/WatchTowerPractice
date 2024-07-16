import axios from 'axios';

const API_URL = 'http://localhost:5003/';
const token = localStorage.getItem('token')?.slice(1).slice(0, -1);

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
    console.log(response.data.data.jwtToken);
    if (response.data.data.jwtToken) {
      localStorage.setItem('token', JSON.stringify(response.data.data.jwtToken));
    }
    return response.data;
  })
  .catch((error) => {
    console.error("Login error: ", error);
    throw error;
  });


const getCameras = () => {
  return axios.get(`${API_URL}camera/get-cameras`, {
    headers: { Authorization: `Bearer ${token}` }
  });
};

const registerCamera = (title: string, ip: string, name: string, password: string) => {
  console.log(`Bearer ${token}`)
  return axios.post(`${API_URL}camera/register-camera`, { ip, name, password }, {
    headers: { Authorization: `Bearer ${token}`}
  })
};

const startStream = (ip: string, name: string, password: string) => 
  axios.post(`${API_URL}stream/start-stream`, { ip, name, password }, {
    headers: { Authorization: `Bearer ${token}` }
  }).then((response) => {
    console.log(response);
    console.log(response.data.data);
    return response.data;
  }).catch((error) => {
    console.error("Start stream error: ", error);
    throw error;
  });

const stopStream = () => {
  return axios.get(`${API_URL}stream/stop-stream`)
}
const updateCamera = (id: number, updatedCamera: { title: string;  id: number;  name: string;  ip: string;  password: string;}) => 
  axios.post(`${API_URL}camera/update-camera`, { id, updatedCamera }, {
  headers: { Authorization: `Bearer ${token}` }
}).then((response) => {
  console.log(response);
  console.log(response.data.data);
  return response.data;
}).catch((error) => {
  console.error("error: ", error);
  throw error;
});
const DeleteCamera = (id: number) => 
  axios.post(`${API_URL}camera/delete-camera`, { id }, {
  headers: { Authorization: `Bearer ${token}` }
}).then((response) => {
  console.log(response);
  console.log(response.data.data);
  return response.data;
}).catch((error) => {
  console.error("error: ", error);
  throw error;
});

export default {
  register,
  login,
  getCameras,
  registerCamera,
  startStream,
  stopStream,
  updateCamera,
  deleteCamera: DeleteCamera
};
