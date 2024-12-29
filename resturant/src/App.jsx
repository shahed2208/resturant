import React from 'react';
import Menu from './components/Menu';
import OrderForm from './components/OrderForm';
import TableList from './components/TableList';

function App() {
  return (
    <div className="App">
      <h1>Restaurant Management System</h1>
      <Menu />
      <OrderForm />
      <TableList />
    </div>
  );
}

export default App;