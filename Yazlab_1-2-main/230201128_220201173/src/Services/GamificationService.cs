﻿using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;

public class GamificationService
{
    private readonly ApplicationDbContext _context;

    public GamificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddPoints(int userId, string activityType, int points)
    {
        var userPoints = _context.UserPoints.FirstOrDefault(up => up.UserId == userId);
        if (userPoints == null)
        {
            userPoints = new UserPoints { UserId = userId, TotalPoints = 0 };
            _context.UserPoints.Add(userPoints);
        }

        // Güncelleme
        switch (activityType)
        {
            case "Participation":
                userPoints.EventParticipationPoints += points;
                break;
            case "Creation":
                userPoints.EventCreationPoints += points;
                break;
            case "Bonus":
                userPoints.BonusPoints += points;
                break;
        }
        userPoints.TotalPoints = userPoints.EventParticipationPoints + userPoints.EventCreationPoints + userPoints.BonusPoints;

        // Aktivite Geçmişine Ekle
        _context.ActivityHistory.Add(new ActivityHistory
        {
            UserId = userId,
            ActivityType = activityType,
            PointsEarned = points
        });

        _context.SaveChanges();
    }

    public UserPoints GetUserPoints(int userId)
    {
        return _context.UserPoints.FirstOrDefault(up => up.UserId == userId);
    }
}
