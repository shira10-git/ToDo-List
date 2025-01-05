import axios from 'axios';

// קביעת כתובת ברירת מחדל
//const apiUrl = process.env.REACT_APP_API_URL;
console.log("REACT_APP_API_URL:", process.env.REACT_APP_API_URL);
const apiUrl = https://todo-list-server-ofps.onrender.com;
//axios.defaults.baseURL = apiUrl;
//console.log("axios.defaults.baseURL:", axios.defaults.baseURL);
console.log("apiUrl:", apiUrl);
// Interceptor לניהול שגיאות
axios.interceptors.response.use(
  response => response,
  error => {
    console.error("API Error:", {
      message: error.message,
      response: error.response ? error.response.data : "No response data",
      status: error.response ? error.response.status : "No status",
    });
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get(apiUrl+"/tasks");
      return result.data;
    } catch (error) {
      if (!error.response) {
        console.error("Network Error: Server might be down or unreachable.");
      }
      throw error;
    }
  },
  addTask: async (name) => {
    try {
      const result = await axios.post(apiUrl+"/tasks", { name, isComplete: false });
      return result.data;
    } catch (error) {
      console.error("Error adding task:", error);
      throw error;
    }
  },
  setCompleted: async (id, name, isComplete) => {
    try {
      const result = await axios.put(apiUrl+`/tasks/${id}`, { name, isComplete });
      return result.data;
    } catch (error) {
      console.error("Error updating task:", error);
      throw error;
    }
  },
  deleteTask: async (id) => {
    try {
      const result = await axios.delete(apiUrl+`/tasks/${id}`);
      return result.data;
    } catch (error) {
      console.error("Error deleting task:", error);
      throw error;
    }
  }
};
