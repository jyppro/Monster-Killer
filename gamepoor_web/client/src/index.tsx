import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from './app'; // App.tsx의 기본 라우팅을 포함하는 컴포넌트

ReactDOM.render(
  <BrowserRouter>
    <Routes>
      <Route path="/*" element={<App />} />
    </Routes>
  </BrowserRouter>,
  document.getElementById('root')
);
