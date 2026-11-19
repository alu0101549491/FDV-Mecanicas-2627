using UnityEngine;

namespace GameplayPhysics
{
    public class Contact2D
    {
        public static bool IsContactFromAbove(Collision2D collision)
        {
            return IsContactFromAbove(collision, Vector2.up);
        }

        public static bool IsContactFromAbove(Collision2D collision, Vector2 contactNormal)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Dot(contact.normal, contactNormal) > 0f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}