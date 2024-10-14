// src/components/Contact.tsx
import React from 'react';
import '../styles/contact.css'; // 스타일 파일 추가

const Contact: React.FC = () => {
  return (
    <div className="contact-container">
      <h1>연락처</h1>
      <p>궁금한 사항이 있으시면 아래 정보를 참고해주세요.</p>
      <h2>이메일</h2>
      <p>youngchan468@naver.com</p>

      <h2>전화</h2>
      <p>010-1234-5434</p>

      <h2>소셜 미디어</h2>
      <p>
        <a href="https://twitter.com/MonsterKiller">Twitter</a><br />
        <a href="https://facebook.com/MonsterKiller">Facebook</a>
      </p>
    </div>
  );
};

export default Contact;
