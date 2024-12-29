import React, { useState, useEffect } from 'react';
import axios from 'axios';

function Menu() {
  const [menuItems, setMenuItems] = useState([]);

  useEffect(() => {
    const fetchMenu = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/menu'); 
        setMenuItems(response.data);
      } catch (error) {
        console.error("Error fetching menu:", error);
       
      }
    };

    fetchMenu();
  }, []); 

  return (
    <div>
      <h1>Menu</h1>
      <ul>
        {menuItems.map(item => (
          <li key={item.id}>
            {item.name} - {item.price}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Menu;