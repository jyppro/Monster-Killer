// src/components/Main.tsx
import React from 'react';

const Main: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <main>
      {children}
    </main>
  );
};

export default Main;


import '../styles/sign-up.css';


