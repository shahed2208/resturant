import React, { useState, useEffect } from 'react';
import axios from 'axios';

function OrderForm() {
  const [order, setOrder] = useState({
    tableNumber: '',
    items: [],
    status: 'جديد' // حالة الطلب الافتراضية
  });
  const [menuItems, setMenuItems] = useState([]);
  const [selectedItems, setSelectedItems] = useState([]);
  const [message, setMessage] = useState(''); // لعرض رسائل للمستخدم
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMenu = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/menu'); // رابط الـ API لقائمة الطعام
        setMenuItems(response.data);
      } catch (error) {
        console.error("Error fetching menu:", error);
        setError("حدث خطأ في جلب قائمة الطعام.");
      }
    };

    fetchMenu();
  }, []);

  const handleInputChange = (e) => {
    setOrder({ ...order, [e.target.name]: e.target.value });
  };

  const handleSelectItem = (item) => {
    if (selectedItems.find(i => i.id === item.id)) {
      setSelectedItems(selectedItems.filter(i => i.id !== item.id));
      setOrder({ ...order, items: order.items.filter(i => i.id !== item.id) });
    } else {
      setSelectedItems([...selectedItems, item]);
      setOrder({ ...order, items: [...order.items, {id: item.id, name: item.name, price: item.price}] });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    setError(null);

    if (order.tableNumber === '') {
      setError("الرجاء إدخال رقم الطاولة.");
      return;
    }
    if (order.items.length === 0) {
      setError("الرجاء اختيار عناصر من القائمة.");
      return;
    }

    try {
      const response = await axios.post('http://localhost:5000/api/orders', order);
      setMessage('تم إنشاء الطلب بنجاح!');
      setOrder({
        tableNumber: '',
        items: [],
        status: 'جديد'
      });
      setSelectedItems([]);
    } catch (err) {
      console.error('Error creating order:', err);
      if (err.response) {
        setError(`حدث خطأ: ${err.response.data}`); // عرض رسالة الخطأ من الـ API
      } else {
        setError('حدث خطأ غير متوقع.');
      }
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error-message">{error}</div>}
      {message && <div className="success-message">{message}</div>}

      <label htmlFor="tableNumber">رقم الطاولة:</label>
      <input
        type="number"
        id="tableNumber"
        name="tableNumber"
        value={order.tableNumber}
        onChange={handleInputChange}
      />

      <h2>قائمة الطعام:</h2>
      <ul>
        {menuItems.map((item) => (
          <li key={item.id} onClick={() => handleSelectItem(item)} style={{cursor: 'pointer', backgroundColor: selectedItems.find(i => i.id === item.id) ? 'lightgreen' : 'white'}}>
            {item.name} - {item.price}
          </li>
        ))}
      </ul>

      <button type="submit">إرسال الطلب</button>
    </form>
  );
}

export default OrderForm;