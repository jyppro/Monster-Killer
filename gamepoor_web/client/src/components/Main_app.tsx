import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Header from './Header';
import Overview from './Overview';
import Team from './Team';
import Mission from './Mission';
import Screenshot1 from './Screenshot1';
import Screenshot2 from './Screenshot2';
import Screenshot3 from './Screenshot3';
import Events from './Events';
import Forums from './Forums';
import Groups from './Groups';
import Faqs from './Faqs';
import Contact from './Contact';
import Feedback from './Feedback';

const Mainapp: React.FC = () => {
  return (
    <Router>
      <Header />
      <Routes>
        {/* 소개 관련 라우트 */}
        <Route path="/" element={<Overview />} />
        <Route path="/intro/team" element={<Team />} />
        <Route path="/intro/mission" element={<Mission />} />

        {/* 스크린샷 관련 라우트 */}
        <Route path="/screenshots/1" element={<Screenshot1 />} />
        <Route path="/screenshots/2" element={<Screenshot2 />} />
        <Route path="/screenshots/3" element={<Screenshot3 />} />

        {/* 커뮤니티 관련 라우트 */}
        <Route path="/community/events" element={<Events />} />
        <Route path="/community/forums" element={<Forums />} />
        <Route path="/community/groups" element={<Groups />} />

        {/* 지원 관련 라우트 */}
        <Route path="/support/faqs" element={<Faqs />} />
        <Route path="/support/contact" element={<Contact />} />
        <Route path="/support/feedback" element={<Feedback />} />

        {/* 홈 */}
        <Route path="/" element={<Overview />} />
      </Routes>
    </Router>
  );
};

export default Mainapp;
