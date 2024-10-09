import React, { useState } from 'react';
import '../styles/feedback.css'; // 스타일 파일 추가

const Feedback: React.FC = () => {
  const [message, setMessage] = useState('');
  
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("피드백 제출:", message);
    setMessage('');
  };

  return (
    <div className="feedback-container">
      <h1>피드백</h1>
      <p>저희 게임에 대한 피드백을 남겨주세요!</p>
      <form onSubmit={handleSubmit}>
        <textarea 
          value={message} 
          onChange={(e) => setMessage(e.target.value)} 
          placeholder="피드백 내용을 입력하세요."
          rows={5}
          required
        />
        <button type="submit">제출</button>
      </form>
    </div>
  );
};

export default Feedback;
