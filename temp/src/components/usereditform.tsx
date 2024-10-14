import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import styles from '../styles/userEdit.module.css';

const UserEditForm: React.FC = () => {
  const [username, setUsername] = useState('');
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    axios.get('http://localhost:5000/api/users/me', {
      withCredentials: true  // 쿠키를 포함시켜 세션을 유지
    })
    .then(response => {
      const { username } = response.data;
      setUsername(username);
    })
    .catch(error => {
      setError('유저 정보를 가져오는데 실패했습니다.');
    });
  }, []);
  
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await axios.put('http://localhost:5000/api/users/me', { currentPassword, newPassword }, { withCredentials: true });
      if (response.status === 200) {
        setSuccess('비밀번호가 성공적으로 변경되었습니다.');
        setCurrentPassword('');
        setNewPassword('');
      }
    } catch (err) {
      setError('비밀번호 변경에 실패했습니다.');
    }
  };

  return (
    <div className={styles.container}>
      <form onSubmit={handleSubmit} className={styles.userEditForm}>
        <h1 className={styles.userEdit_title}>비밀번호 변경</h1>
        <div className={styles.inputGroup}>
          <label className={styles.label} htmlFor="username">아이디</label>
          <input
            type="text"
            value={username}
            readOnly // 읽기 전용 설정
            id="username"
            className={styles.userId}
          />
        </div>
        <div className={styles.inputGroup}>
          <label className={styles.label} htmlFor="currentPassword">현재 비밀번호</label>
          <input
            type="password"
            value={currentPassword}
            onChange={(e) => setCurrentPassword(e.target.value)}
            placeholder="현재 비밀번호"
            id="currentPassword"
            className={styles.password}
          />
        </div>
        <div className={styles.inputGroup}>
          <label className={styles.label} htmlFor="newPassword">새로운 비밀번호</label>
          <input
            type="password"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            placeholder="새로운 비밀번호"
            id="newPassword"
            className={styles.password}
          />
        </div>
        {error && <p className={styles.error}>{error}</p>}
        {success && <p className={styles.success}>{success}</p>}
        <button type="submit" className={styles.save_button}>변경</button>
      </form>
    </div>
  );
};

export default UserEditForm;
