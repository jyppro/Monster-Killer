import React, { useState } from "react";
import "../styles/feedback.css";
// import video1 from "../videos/bg_gameplay.mp4";
import video1 from "../video/background.mp4";

const Feedback: React.FC = () => {
  const [message, setMessage] = useState("");
  const [feedbacks, setFeedbacks] = useState<string[]>([]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (message.trim()) {
      setFeedbacks([...feedbacks, message]); // 제출한 피드백을 리스트에 추가
      setMessage(""); // 입력 필드 초기화
    }
  };

  return (
    <div className="feedback-container">
      <div className="bg-video">
        <video className="bg-video__content" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>

      <div className="feedback-content">
        <h1>Monster Killer 이용 피드백</h1>
        <p>게임에 대한 추가의견을 남겨주세요!</p>
        <form onSubmit={handleSubmit}>
          <textarea
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            placeholder="피드백 내용을 입력하세요."
            rows={5}
            required
          />
          <button type="submit" className="submit-button">
            제출
          </button>
        </form>

        <div className="feedback-list">
          {feedbacks.length > 0 ? (
            feedbacks.map((feedback, index) => (
              <div key={index} className="feedback-item">
                {index + 1}. {feedback}
              </div>
            ))
          ) : (
            <p>제출된 피드백이 없습니다.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Feedback;
