using DG.Tweening;
using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Entities
{
    /// <summary>
    /// A unit on the board: one sprite renderer that walks. Pooled (group "units"), so the same objects
    /// are reused as units come and go. Walking is a tween along the planned cells at a steady speed -
    /// how the unit moves is presentation; where it goes was decided before it got here. Selected, its
    /// sprite is tinted - a vertex colour, so a tinted unit still batches with the rest.
    /// </summary>
    public class BoardUnit : PoolableItem
    {
        [SerializeField] private SpriteRenderer _renderer;

        /// <summary>The unit starts stepping towards this waypoint - the centre of the cell it is walking into.</summary>
        public System.Action<Vector3> StepStarted;

        private Tween _walk;

        /// <summary>
        /// Draws the sprite over a world rect - the cell the unit stands on. The sprite is scaled to the
        /// rect, so a unit covers one cell whatever the sprite's own pixel size.
        /// </summary>
        public void Show(Sprite sprite, Rect area)
        {
            _renderer.sprite = sprite;

            Vector2 spriteSize = sprite.bounds.size;
            transform.position = area.center;
            transform.localScale = new Vector3(area.width / spriteSize.x, area.height / spriteSize.y, 1f);
        }

        /// <summary>
        /// Walks from where the unit is through every waypoint, the last where it stops, replacing any walk under way.
        /// A first waypoint the unit already stands on is skipped - a unit at rest starts from its own cell.
        /// </summary>
        /// <param name="waypoints">The centre of every cell to walk through, one cell apart.</param>
        /// <param name="unitsPerSecond">Walking speed in world units.</param>
        public void MoveAlong(Vector3[] waypoints, float unitsPerSecond)
        {
            _walk?.Kill();

            if (waypoints.Length > 0 && waypoints[0] == transform.position)
                waypoints = waypoints[1..];

            if (waypoints.Length == 0) return;

            // A path tween counts where it starts as waypoint 0, so when waypoint i is reached the unit is on its
            // way to waypoints[i] - the last index means it has arrived.
            Vector3[] steps = waypoints;
            _walk = transform.DOPath(steps, unitsPerSecond, PathType.Linear)
                             .SetSpeedBased()
                             .SetEase(Ease.Linear)
                             .OnWaypointChange(reached =>
                             {
                                 if (reached < steps.Length) StepStarted?.Invoke(steps[reached]);
                             });
        }

        public void ShowSelected(Color tint) => _renderer.color = tint;

        public void ShowDeselected() => _renderer.color = Color.white;

        public override void OnReturnToPool()
        {
            _walk?.Kill();
            _walk = null;
            StepStarted = null;
            _renderer.sprite = null;
            _renderer.color = Color.white;
        }
    }
}
