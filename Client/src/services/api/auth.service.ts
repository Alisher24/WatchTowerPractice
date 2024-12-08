import axios from 'axios';

const API_URL = 'http://localhost:5003/';
const token = localStorage.getItem('token')?.slice(1).slice(0, -1);
axios.defaults.headers.common['Authorization'] = "Bearer " + token;

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

const login = (email, password) => axios
  .post(`${API_URL}auth/Login`, {
    email: email,
    password: password,
  })
  .then((response) => {
    console.log(response);
    console.log(response.data.data.jwtToken);
    if (response.data.data.jwtToken) {
      localStorage.setItem('token', JSON.stringify(response.data.data.jwtToken));
      axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem('token')?.slice(1).slice(0, -1);
    }
    return response.data;
  })
  .catch((error) => {
    console.error("Login error: ", error);
    throw error;
  });


const getCameras = () => {
  return axios.get(`${API_URL}camera/get-cameras`);
};

const registerCamera = (title: string, ip: string, name: string, password: string) => {
  console.log(`Bearer ${token}`)
  return axios.post(`${API_URL}camera/register-camera`, {title, ip, name, password})
};

const startStream = (ip: string, name: string, password: string) =>
  axios.post(`${API_URL}stream/start-stream`, {ip, name, password},
  ).then((response) => {
    console.log(response);
    console.log(response.data.data);
    return response.data;
  }).catch((error) => {
    console.error("Start stream error: ", error);
    throw error;
  });

const updateCamera = (updatedCamera: { name: string; id: number; title: string; ip: string; password: string; }) => {
  console.log(updatedCamera)
  return axios.put(`${API_URL}camera/update-camera`, {
    id: updatedCamera.id,
    title: updatedCamera.title,
    ip: updatedCamera.ip,
    password: updatedCamera.password,
    name: updatedCamera.name,
    },
  ).then((response) => {
    console.log(response);
    console.log(response.data.data);
    return response.data;
  }).catch((error) => {
    console.error("error: ", error);
    throw error;
  });
};
const deleteCamera = (id: number) =>
  axios.delete(`${API_URL}camera/delete-camera/${id}`).then((response) => {
    console.log(response);
    console.log(response.data.data);
    return response.data;
  }).catch((error) => {
    console.error("error: ", error);
    throw error;
  });

const startStreamByID = (id: string) =>
  axios.get(`${API_URL}stream/${id}`,
  ).then((response) => {
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


export default {
  register,
  login,
  getCameras,
  registerCamera,
  startStream,
  startStreamByID,
  stopStream,
  updateCamera,
  deleteCamera,
};
