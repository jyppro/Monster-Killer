// Footer.tsx
import React from 'react';
import '../styles/footer.css'; // 푸터 CSS 파일

const Footer: React.FC = () => {
  return (
    <footer className="footer">
      <p>&copy; 2024 Gampure All Rights Reserved.</p>
      <p>
        <a href="/privacy-policy">개인정보 보호정책</a> | 
        <a href="/license"> 사용권</a> | 
        <a href="/terms"> Gampure 이용 약관</a> | 
        <a href="/cookies"> 쿠키</a>
      </p>
    </footer>
  );
};

export default Footer;
