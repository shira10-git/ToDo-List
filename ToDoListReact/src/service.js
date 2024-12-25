import axios from 'axios';

// הגדרת כתובת ה-API כ-default
//const apiUrl = "https://todo-list-server-ofps.onrender.com";
// const apiUrl = process.env.REACT_APP_API_URL;


//axios.defaults.baseURL = apiUrl;  // הגדרת כתובת ברירת המחדל לכל הקריאות
//console.log("axios.defaults.baseURL",axios.defaults.baseURL)
// הוספת interceptor לתפיסת שגיאות
axios.interceptors.response.use(
  response => response, // אם הקריאה הצליחה, מחזירים את התשובה כרגיל
  error => {
    console.error("API Error : ", error.response ? error.response.data : error.message);
    return Promise.reject(error); // מחזירים את השגיאה למעלה
  }
);

export default {
  // שליפת כל המשימות
  getTasks: async () => {
    try {
      const result = await axios.get("https://todo-list-server-ofps.onrender.com/tasks");
      return result.data;
    } catch (error) {
      console.error("Error fetching tasks:", error);
      throw error;
    }
  },

  // הוספת משימה חדשה
  addTask: async (name) => {
    try {
      const result = await axios.post("https://todo-list-server-ofps.onrender.com/tasks", { name, isComplete: false });
      return result.data;
    } catch (error) {
      console.error("Error adding task:", error);
      throw error;
    }
  },

// עדכון סטטוס של משימה
setCompleted: async (id, name, isComplete) => {
  try {
      // שולחים גם את שם המשימה בנוסף לסטטוס
      const result = await axios.put(`https://todo-list-server-ofps.onrender.com/tasks/${id}`, { name, isComplete });
      return result.data;
  } catch (error) {
      console.error("Error updating task:", error);
      throw error;
  }
},


  // מחיקת משימה
  deleteTask: async (id) => {
    try {
      const result = await axios.delete(`https://todo-list-server-ofps.onrender.com/tasks/${id}`);
      return result.data;
    } catch (error) {
      console.error("Error deleting task:", error);
      throw error;
    }
  }
};
