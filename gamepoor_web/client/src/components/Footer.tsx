import React from 'react';
import '../styles/footer.css'; // 푸터 CSS 파일

const Footer: React.FC = () => {
  return (
    <footer className="footer">
      <div className="copyright">
        <p>&copy; 2024 Gampure All Rights Reserved. 모든 로고는 겜푸어에 해당하는 팀의 재산입니다.</p>
      </div>
      <div className="links">
        <p>
          <a href="/">Monster Killer</a> | 
          <a href="/privacy-policy"> 개인정보 보호정책</a> | 
          <a href="/license"> 사용권</a> | 
          <a href="/terms"> Gampure 이용 약관</a> | 
          <a href="/cookies"> 쿠키</a>
        </p>
      </div>
    </footer>
  );
};

export default Footer;
