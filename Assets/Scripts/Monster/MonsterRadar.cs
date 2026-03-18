//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MonsterRadar : MonoBehaviour
//{
//    private Monster _monster;

//    // Start is called before the first frame update
//    void Start()
//    {
//        _monster = GetComponentInParent<Monster>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //Debug.DrawRay(transform.position, transform.forward * 1.2f, Color.yellow);
//        //if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.2f))
//        if (Physics.SphereCast(transform.position, 1.0f, transform.forward, out RaycastHit hit, 1.0f))
//        {
//            if(hit.transform.CompareTag("BaseWall") || hit.transform.CompareTag("Player"))
//            {
//                _monster.NearbyTheBase = true;
//                _monster.SetAttackTarget(hit.transform);
//            }
//        }
//        else
//        {
//            _monster.SetAttackTarget(null);
//        }
//    }

//    void OnDrawGizmos()
//    {
//        // Draw a yellow sphere at the transform's position
//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(transform.position, 1.0f);
//    }

//    //private void OnTriggerEnter(Collider coll)
//    //{
//    //    //var target = coll.GetComponent<MonsterTarget>();

//    //    //if(target)
//    //    //{
//    //    //    _monster.NearbyTheBase = true;
//    //    //    _monster.SetAttackTarget(coll.gameObject.transform);
//    //    //}

//    //    if (coll.gameObject.tag.Equals("BaseWall") || coll.gameObject.tag.Equals("Player"))
//    //    {
//    //        _monster.NearbyTheBase = true;
//    //        _monster.SetAttackTarget(coll.gameObject.transform);
//    //    }
//    //}

//    //private void OnTriggerExit(Collider coll)
//    //{
//    //    //var target = coll.GetComponent<MonsterTarget>();

//    //    //if (target)
//    //    //{
//    //    //    _monster.SetAttackTarget(null);
//    //    //}

//    //    if (coll.gameObject.tag.Equals("BaseWall") || coll.gameObject.tag.Equals("Player"))
//    //    {
//    //        _monster.SetAttackTarget(null);
//    //    }
//    //}
//}
