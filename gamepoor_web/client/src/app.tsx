import React from 'react';
import { Route, Routes } from 'react-router-dom';
import LoginForm from './components/LoginForm';
import SignUpForm from './components/SignForm';
import Game from './components/game';
import UserEditForm from './components/usereditform';


const App: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<LoginForm />} />
      <Route path="/signup" element={<SignUpForm />} />
      <Route path="/game" element={<Game />} />
      <Route path="/edit" element={<UserEditForm />} />
    </Routes>
  );
};

export default App;
