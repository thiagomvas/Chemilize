using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Conveyor : MonoBehaviour
{
    public bool forward;
    private int directionsIndex;
    [SerializeField]private List<Vector3> directions = new List<Vector3>();
    public Transform other;
    [SerializeField] private float baseSpeed;
    public List<GameObject> checks = new List<GameObject>();
    [SerializeField] private List<GameObject> sidewalls = new List<GameObject>();

    private void Start()
    {
        CheckSide(true);
    }

    private void Update()
    {
        if (forward) { Debug.Log(this.transform.forward); forward = false; }
    }
    public void CheckSide(bool checkSubsequentConveyors)
    {
        directions.Clear();
        foreach (GameObject wall in sidewalls) wall.SetActive(true);

        for (int i = 0; i < checks.Count; i++)
        {
            foreach (Collider col in Physics.OverlapSphere(checks[i].transform.position, 0.1f))
            {
                Component[] comps = col.GetComponents<Component>();
                for (int j = 0; j < comps.Length; j++)
                {
                    switch (comps[j])
                    {
                        case (Conveyor con):
                            if (checkSubsequentConveyors) con.CheckSide(false);
                            sidewalls[i].SetActive(false);
                            if (i == 2 && con.transform.forward == -this.transform.right) break;
                            if (i == 3 && con.transform.forward == this.transform.right) break;
                            if (i != 1) directions.Add((con.transform.position - this.transform.position).normalized);
                            break;
                        case (StorageTank tank):
                            sidewalls[i].SetActive(false);
                            if (i == 0) directions.Add((tank.transform.position - this.transform.position).normalized);
                            break;
                        case (Crafter craft):
                            sidewalls[i].SetActive(false);
                            if (i == 0) directions.Add((craft.transform.position - this.transform.position).normalized);
                            break;
                        case (Seller seller):
                            sidewalls[i].SetActive(false);
                            if (i == 0) directions.Add((seller.transform.position - this.transform.position).normalized);
                            break;
                        case (Turret turret):
                            sidewalls[i].SetActive(false);
                            if (i == 0) directions.Add((turret.transform.position - this.transform.position).normalized);
                            break;
                    }
                }
            }
        }
        directions = directions.Distinct().ToList();
    }
    public IEnumerator CheckSides(float time, bool checkSubsequentConveyors)
    {
        yield return new WaitForSeconds(time);
        CheckSide(checkSubsequentConveyors);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Element>() == null) return;
        other.GetComponent<Rigidbody>().isKinematic = true;
        directionsIndex++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Element>() == null) return;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float x, y, z, rx, rz;
            x = this.transform.position.x;
            y = this.transform.position.y;
            z = this.transform.position.z;
            rx = rb.transform.position.x; 
            rz = rb.transform.position.z;

            if (directions.Count == 0) return;

            if (x == Mathf.Round(rx * 10) / 10 || z == Mathf.Round(rz * 10) / 10) rb.MovePosition(rb.transform.position + directions[directionsIndex % directions.Count] * baseSpeed * Time.deltaTime);
            else  rb.position = Vector3.Lerp(rb.position, Vector3.down * 0.15f + this.transform.position + directions[directionsIndex % directions.Count] * (2 * baseSpeed), Time.deltaTime/2);

            //if (x == Mathf.Round(rx * 10) / 10 || z == Mathf.Round(rz * 10) / 10) rb.MovePosition(rb.transform.position + this.transform.forward * baseSpeed * Time.deltaTime);
            //else //rb.MovePosition(rb.transform.position + (new Vector3(x, y, z) - new Vector3(rx, y, rz)) * baseSpeed * Time.deltaTime * m);
            //    rb.position = Vector3.Lerp(rb.position, this.transform.position + this.transform.forward, Time.deltaTime * 2);

        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(Vector3 v in directions)
        {
            Gizmos.DrawCube(v + this.transform.position + Vector3.up, Vector3.one);
        }

    }
}
