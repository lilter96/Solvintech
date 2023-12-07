import React from 'react';
import './App.css';
import AuthComponent from './components/Auth';
import Header from './components/Header';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

const App: React.FC = () => {
  return (
    <div>
      <Header />
      <AuthComponent />
      <ToastContainer />
    </div>
  );
};

export default App;