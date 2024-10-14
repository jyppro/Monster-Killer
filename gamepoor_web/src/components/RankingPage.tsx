import React, { useState, useEffect } from "react";
import "../styles/rank.css";

const RankingPage = () => {
  const [rankingData, setRankingData] = useState([
    { id: 1, name: "A1", score: 95 },
    { id: 2, name: "B2", score: 85 },
    { id: 3, name: "C3", score: 80 },
    { id: 4, name: "D4", score: 90 },
    { id: 5, name: "E5", score: 70 },
  ]);

  useEffect(() => {
    const sortedData = [...rankingData].sort((a, b) => b.score - a.score);
    setRankingData(sortedData);
  }, [rankingData]);

  return (
    <div className="container">
      <h1>Ranking</h1>
      <table>
        <thead>
          <tr>
            <th>Rank</th>
            <th>Name</th>
            <th>Score</th>
          </tr>
        </thead>
        <tbody>
          {rankingData.map((player, index) => (
            <tr key={player.id}>
              <td>{index + 1}</td>
              <td>{player.name}</td>
              <td>{player.score}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default RankingPage;
